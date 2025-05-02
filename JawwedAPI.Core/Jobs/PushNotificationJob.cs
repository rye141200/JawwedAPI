using FirebaseAdmin.Messaging;
using Hangfire;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.Enums;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.NotificationInterfaces;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using Microsoft.Extensions.Logging;

namespace JawwedAPI.Core.Jobs
{
    public class PushNotificationJob(
        IGoalsService goalsService,
        INotificationService notifier,
        IGenericRepository<ApplicationUser> users,
        ILogger<PushNotificationJob> log
    )
    {
        // Called once a day by Hangfire
        public async Task CheckAndNotifyAllUsersAsync(CancellationToken _ = default)
        {
            log.LogInformation("Starting daily notification check for all users");

            var subs = await users.Find(users =>
                users.EnableNotifications && !string.IsNullOrWhiteSpace(users.DeviceToken)
            );
            log.LogInformation("Found {UserCount} users with notifications enabled", subs.Count());

            foreach (var user in subs)
            {
                log.LogDebug("Processing notifications for user {UserId}", user.UserId);

                var goals = (await goalsService.GetAllUserGoalsAsync(user.UserId)).Where(g =>
                    g.Status == GoalStatus.InProgress.ToString()
                );

                log.LogDebug("User {UserId} has {GoalCount} goals", user.UserId, goals.Count());
                foreach (var goal in goals)
                {
                    log.LogDebug(
                        "Processing goal {GoalId} for user {UserId}",
                        goal.GoalId,
                        user.UserId
                    );

                    await SendMissedDaysAsync(user.DeviceToken!, goal);
                    ScheduleInProgressReminders(user.UserId, user.DeviceToken!, goal);
                }
            }
        }

        private async Task SendMissedDaysAsync(string token, GoalResponse goal)
        {
            log.LogDebug("Checking missed days for goal {GoalId}", goal.GoalId);

            var missed = goal
                .ReadingSchedule.Where(session =>
                    session.Status == ReadingSessionStatus.Missed.ToString()
                )
                .Select(session => session.DayNumber)
                .ToList();
            if (!missed.Any())
            {
                log.LogDebug("No missed days found for goal {GoalId}", goal.GoalId);
                return;
            }

            log.LogInformation(
                "Sending missed days notification for goal {GoalId}. Missed days: {MissedDays}",
                goal.GoalId,
                string.Join(", ", missed)
            );

            Message mssage = new Message
            {
                Token = token,
                Notification = new Notification()
                {
                    Title = $"Missed Days {goal.Title}",
                    Body = $"You’ve missed days: {string.Join(", ", missed)}",
                },
            };
            await notifier.SendAsync(mssage);
            log.LogDebug(
                "Successfully sent missed days notification for goal {GoalId}",
                goal.GoalId
            );
        }

        private void ScheduleInProgressReminders(Guid userId, string token, GoalResponse goal)
        {
            log.LogDebug("Checking in-progress sessions for goal {GoalId}", goal.GoalId);
            var inProgressSessions = goal
                .ReadingSchedule.Where(s => s.Status == ReadingSessionStatus.InProgress.ToString())
                .ToList();

            if (!inProgressSessions.Any())
            {
                log.LogDebug("No in-progress sessions found for goal {GoalId}", goal.GoalId);
                return;
            }

            log.LogInformation(
                "Scheduling reminders for {Count} in-progress sessions for goal {GoalId}",
                inProgressSessions.Count,
                goal.GoalId
            );

            foreach (var session in inProgressSessions)
            {
                log.LogDebug(
                    "Scheduling morning reminder for day {DayNumber}, goal {GoalId}",
                    session.DayNumber,
                    goal.GoalId
                );
                ScheduleAt(
                    userId,
                    token,
                    $"Time to catch up {goal.Title}",
                    $"Day {session.DayNumber} is in progress.",
                    goal.GoalId,
                    session.DayNumber,
                    session.ScheduledDate, // pass through the date you already have
                    goal.ReminderTime // TimeSpan exactly as the user specified
                );
            }
        }

        private void ScheduleAt(
            Guid userId,
            string token,
            string title,
            string body,
            Guid goalId,
            int dayNumber,
            DateTime sessionDate, // your date (no time part)
            TimeSpan reminderTime // your TimeSpan-only reminder time
        )
        {
            // 1) Build a local DateTime for sessionDate at reminderTime
            var localDateTime = DateTime.SpecifyKind(
                sessionDate.Date.Add(reminderTime),
                DateTimeKind.Local
            );

            // 2) Convert that local time to UTC
            var fireUtc = localDateTime.ToUniversalTime();

            // 3) If it’s already past, run in 1 minute
            var nowUtc = DateTime.UtcNow;
            if (fireUtc <= nowUtc)
                fireUtc = nowUtc.AddMinutes(1);

            // 4) Compute delay
            var delay = fireUtc - nowUtc;

            // 5) Schedule the one‐off job
            var msg = new Message
            {
                Token = token,
                Notification = new Notification { Title = title, Body = body },
            };
            string jobId = BackgroundJob.Schedule<INotificationService>(
                svc => svc.SendAsync(msg),
                delay
            );

            // 6) Stamp your unique JobKey
            var jobKey = $"{userId:N}-{goalId:N}-day{dayNumber}";
            JobStorage.Current.GetConnection().SetJobParameter(jobId, "JobKey", jobKey);

            log.LogDebug(
                "Scheduled job {JobId} to fire at {FireUtc:O} (delay {Delay}) key={JobKey}",
                jobId,
                fireUtc,
                delay,
                jobKey
            );
        }
    }
}
