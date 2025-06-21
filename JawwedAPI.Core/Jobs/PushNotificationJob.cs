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

            // Clean up existing jobs first
            await CleanupExistingJobs(jobKey);

            // Calculate reminder time using server timezone (simplified approach)
            var reminderTime = DateTime.Today.Add(goal.ReminderTime);
            var utcReminderTime = reminderTime.ToUniversalTime();

            // Schedule the recurring job to run daily at the specified reminder time
            RecurringJob.AddOrUpdate(
                jobKey,
                () =>
                    SendNotification(
                        user.DeviceToken!,
                        goal.Title,
                        $"حان الوقت لقراة وردك اليومي من تحدي {goal.Title}"
                    ),
                Cron.Daily(utcReminderTime.Hour, utcReminderTime.Minute)
            );

            // Schedule job to check and update goal status after duration
            var endDate = DateTime.UtcNow.AddDays(goal.DurationDays);
            var client = new BackgroundJobClient();
            client.Schedule(
                () => CheckAndUpdateGoalStatusAsync(userId, goalId),
                endDate - DateTime.UtcNow
            );

            log.LogInformation(
                "Scheduled daily reminder job with key {JobKey} for goal {GoalId} at {Hours}:{Minutes} until {EndDate}",
                jobKey,
                goalId,
                utcReminderTime.Hour,
                utcReminderTime.Minute,
                endDate
            );
        }

        // Helper method to clean up existing jobs
        private async Task CleanupExistingJobs(string jobKey)
        {
            try
            {
                // Delete recurring job if exists
                RecurringJob.RemoveIfExists(jobKey);

                // Get all scheduled jobs and clean up matching ones
                var monitoring = JobStorage.Current.GetMonitoringApi();
                var scheduled = monitoring.ScheduledJobs(0, int.MaxValue);
                var conn = JobStorage.Current.GetConnection();

                foreach (var kv in scheduled)
                {
                    string? existingJobKey = conn.GetJobParameter(kv.Key, "JobKey");
                    if (existingJobKey == jobKey)
                    {
                        BackgroundJob.Delete(kv.Key);
                        log.LogDebug(
                            "Deleted existing job {JobId} with key {JobKey}",
                            kv.Key,
                            jobKey
                        );
                    }
                }

                // Clean up any recurring jobs that start with our key
                var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
                foreach (var job in recurringJobs)
                {
                    if (job.Id.StartsWith(jobKey))
                    {
                        RecurringJob.RemoveIfExists(job.Id);
                        log.LogDebug("Deleted recurring job {JobKey}", job.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogWarning(ex, "Error during job cleanup for key {JobKey}", jobKey);
                // Don't throw - cleanup failure shouldn't prevent new job creation
            }
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
            await CleanupExistingJobs(jobKey);
            log.LogInformation("Removed all jobs for goal {GoalId}", goalId);
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
                log.LogInformation("Successfully sent notification to token {Token}", token);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to send notification to token {Token}", token);
                // Don't throw - let the job complete successfully
                // The [AutomaticRetry] attribute will handle retries if needed
            }
        }

        // Method to remove recurring job - used by Hangfire
        public Task RemoveRecurringJob(string jobKey)
        {
            try
            {
                RecurringJob.RemoveIfExists(jobKey);
                log.LogInformation("Successfully removed recurring job {JobKey}", jobKey);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to remove recurring job {JobKey}", jobKey);
                // Don't throw - let the job complete successfully
                return Task.CompletedTask;
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

            try
            {
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
                    await CleanupExistingJobs(jobKey);
                    log.LogInformation("Cleaned up jobs for completed goal {GoalId}", goalId);
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
            catch (Exception ex)
            {
                log.LogError(ex, "Error checking goal status for goal {GoalId}", goalId);
                // Don't throw - let the job complete successfully
            }
        }
    }
}
