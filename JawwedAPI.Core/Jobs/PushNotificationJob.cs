using FirebaseAdmin.Messaging;
using Hangfire;
using Hangfire.Storage;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.Enums;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Exceptions.CustomExceptions;
using JawwedAPI.Core.ServiceInterfaces.NotificationInterfaces;
using Microsoft.Extensions.Logging;
using TimeZoneConverter;

namespace JawwedAPI.Core.Jobs
{
    public class PushNotificationJob(
        INotificationService notifier,
        IGenericRepository<ApplicationUser> users,
        IGenericRepository<Goal> goals,
        ILogger<PushNotificationJob> log
    )
    {
        // Called when a goal is created or updated
        public async Task ScheduleNotificationsForGoalAsync(
            Guid userId,
            Guid goalId,
            CancellationToken _ = default
        )
        {
            log.LogInformation(
                "Scheduling notifications for goal {GoalId} for user {UserId}",
                goalId,
                userId
            );

            ApplicationUser? user = await users.FindOne(u =>
                u.UserId == userId
                && u.EnableNotifications
                && !string.IsNullOrWhiteSpace(u.DeviceToken)
            );

            if (user == null)
                throw new GlobalErrorThrower(
                    404,
                    "User Not Found or Notifications Disabled",
                    "User not found or notifications are not enabled. Please register your device and enable notifications."
                );

            Goal? goal = await goals.FindOne(g => g.GoalId == goalId && g.UserId == userId);

            if (goal == null)
                throw new GlobalErrorThrower(
                    404,
                    "Goal Not Found",
                    "The specified goal was not found for this user."
                );

            if (goal.Status != GoalStatus.InProgress)
            {
                log.LogDebug(
                    "Goal {GoalId} is not in progress, skipping notification scheduling",
                    goalId
                );
                return;
            }

            // Create a unique job key for this goal
            string jobKey = $"{userId:N}-{goalId:N}";

            // Delete any existing job with the same key
            var monitoring = JobStorage.Current.GetMonitoringApi();
            var scheduled = monitoring.ScheduledJobs(0, int.MaxValue);
            var conn = JobStorage.Current.GetConnection();

            // Clean up any existing jobs with old signature
            foreach (var kv in scheduled)
            {
                string? existingJobKey = conn.GetJobParameter(kv.Key, "JobKey");
                if (existingJobKey == jobKey)
                {
                    BackgroundJob.Delete(kv.Key);
                    log.LogDebug("Deleted existing job {JobId} with key {JobKey}", kv.Key, jobKey);
                }
            }

            // Also clean up any recurring jobs with old signature
            var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
            foreach (var job in recurringJobs)
            {
                if (job.Id.StartsWith(jobKey))
                {
                    RecurringJob.RemoveIfExists(job.Id);
                    log.LogDebug("Deleted recurring job {JobKey}", job.Id);
                }
            }

            // Schedule the recurring job to run daily at the specified reminder time
            Message msg = new()
            {
                Token = user.DeviceToken!,
                Notification = new Notification
                {
                    Title = goal.Title,
                    Body = $"حان الوقت لقراة وردك اليومي من تحدي {goal.Title}",
                },
            };

            // Calculate end date for notifications (in UTC)
            var endDate = DateTime.UtcNow.AddDays(goal.DurationDays);

            // Create a DateTimeOffset from the reminder time and convert to UTC
            var reminderTime = new DateTimeOffset(
                DateTime.Today.Add(goal.ReminderTime),
                DateTimeOffset.Now.Offset
            ).ToUniversalTime();

            // Schedule the job to run daily at exactly the specified time
            RecurringJob.AddOrUpdate(
                jobKey,
                () =>
                    SendNotification(
                        user.DeviceToken!,
                        goal.Title,
                        $"حان الوقت لقراة وردك اليومي من تحدي {goal.Title}"
                    ),
                Cron.Daily(reminderTime.Hour, reminderTime.Minute)
            );

            // Schedule job to check and update goal status after duration
            string statusCheckJobKey = $"{jobKey}-status-check";
            var client = new BackgroundJobClient();
            client.Schedule(
                () => CheckAndUpdateGoalStatusAsync(userId, goalId),
                endDate - DateTime.UtcNow
            );

            log.LogInformation(
                "Scheduled daily reminder job with key {JobKey} for goal {GoalId} at {Hours}:{Minutes} until {EndDate}",
                jobKey,
                goalId,
                reminderTime.Hour,
                reminderTime.Minute,
                endDate
            );
        }

        // Called when a goal is completed or deleted
        public async Task DeleteScheduledNotificationsAsync(
            Guid userId,
            Guid goalId,
            CancellationToken _ = default
        )
        {
            log.LogInformation(
                "Deleting scheduled notifications for goal {GoalId} for user {UserId}",
                goalId,
                userId
            );

            string jobKey = $"{userId:N}-{goalId:N}";
            string cleanupJobKey = $"{jobKey}-cleanup";

            // Delete the recurring notification job
            await Task.Run(() => RecurringJob.RemoveIfExists(jobKey));
            log.LogInformation("Removed recurring job {JobKey}", jobKey);

            // Get all scheduled jobs
            var monitoring = JobStorage.Current.GetMonitoringApi();
            var scheduled = monitoring.ScheduledJobs(0, int.MaxValue);
            var conn = JobStorage.Current.GetConnection();

            // Delete any scheduled jobs that match our job key pattern
            foreach (var kv in scheduled)
            {
                string? existingJobKey = conn.GetJobParameter(kv.Key, "JobKey");
                if (existingJobKey?.StartsWith(jobKey) == true)
                {
                    BackgroundJob.Delete(kv.Key);
                    log.LogInformation(
                        "Deleted scheduled job {JobId} with key {JobKey}",
                        kv.Key,
                        existingJobKey
                    );
                }
            }

            // Also check and delete any completed session jobs
            var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
            foreach (var job in recurringJobs)
            {
                if (job.Id.StartsWith(jobKey))
                {
                    RecurringJob.RemoveIfExists(job.Id);
                    log.LogInformation("Deleted recurring job {JobKey}", job.Id);
                }
            }
        }

        // Method to send notification - used by Hangfire
        [AutomaticRetry(Attempts = 3)]
        public async Task SendNotification(string token, string title, string body)
        {
            try
            {
                var message = new Message
                {
                    Token = token,
                    Notification = new Notification { Title = title, Body = body },
                };
                await notifier.SendAsync(message);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to send notification");
                throw new GlobalErrorThrower(
                    500,
                    "Notification Send Failed",
                    "Failed to send notification. Please try again later."
                );
            }
        }

        // Method to remove recurring job - used by Hangfire
        public Task RemoveRecurringJob(string jobKey)
        {
            try
            {
                RecurringJob.RemoveIfExists(jobKey);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to remove recurring job {JobKey}", jobKey);
                throw new GlobalErrorThrower(
                    500,
                    "Job Removal Failed",
                    "Failed to remove scheduled notification. Please try again later."
                );
            }
        }

        // Called when a goal's duration ends or when checking goal status
        public async Task CheckAndUpdateGoalStatusAsync(Guid userId, Guid goalId)
        {
            log.LogInformation(
                "Checking goal status for goal {GoalId} for user {UserId}",
                goalId,
                userId
            );

            var goal = await goals.FindOne(g => g.GoalId == goalId && g.UserId == userId);
            if (goal == null)
            {
                log.LogWarning("Goal {GoalId} not found for user {UserId}", goalId, userId);
                return;
            }

            // If goal is completed, delete all related jobs
            if (goal.Status == GoalStatus.Completed)
            {
                string jobKey = $"{userId:N}-{goalId:N}";
                string statusCheckJobKey = $"{jobKey}-status-check";

                // Delete the recurring notification job
                RecurringJob.RemoveIfExists(jobKey);
                log.LogInformation("Removed recurring job {JobKey} for completed goal", jobKey);

                // Get all scheduled jobs
                var monitoring = JobStorage.Current.GetMonitoringApi();
                var scheduled = monitoring.ScheduledJobs(0, int.MaxValue);
                var conn = JobStorage.Current.GetConnection();

                // Delete any scheduled jobs that match our job key pattern
                foreach (var kv in scheduled)
                {
                    string? existingJobKey = conn.GetJobParameter(kv.Key, "JobKey");
                    if (existingJobKey?.StartsWith(jobKey) == true)
                    {
                        BackgroundJob.Delete(kv.Key);
                        log.LogInformation(
                            "Deleted scheduled job {JobId} with key {JobKey}",
                            kv.Key,
                            existingJobKey
                        );
                    }
                }

                return;
            }

            // Calculate end date
            var endDate = goal.StartDate.AddDays(goal.DurationDays);

            // If goal is still in progress and duration has ended
            if (goal.Status == GoalStatus.InProgress && DateTimeOffset.UtcNow >= endDate)
            {
                goal.Status = GoalStatus.Canceled;
                goals.Update(goal);
                await goals.SaveChangesAsync();
                log.LogInformation(
                    "Updated goal {GoalId} status to Canceled as duration has ended",
                    goalId
                );
            }
        }
    }
}
