using FirebaseAdmin.Messaging;
using Hangfire;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.Enums;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Exceptions.CustomExceptions;
using JawwedAPI.Core.ServiceInterfaces.NotificationInterfaces;
using Microsoft.Extensions.Logging;

namespace JawwedAPI.Core.Services;

public class FcmNotificationService(
    FirebaseMessaging messaging,
    IBackgroundJobClient jobs,
    ILogger<FcmNotificationService> log,
    IGenericRepository<ApplicationUser> userRepository
) : INotificationService
{
    public async Task SendAsync(Message message)
    {
        // Get the user by device token
        var user = await userRepository.FindOne(u => u.DeviceToken == message.Token);
        if (user == null)
            throw new GlobalErrorThrower(
                404,
                "User Not Found",
                "User not found for this device token"
            );

        // Only send if notifications are enabled
        if (!user.EnableNotifications)
        {
            log.LogInformation(
                "Skipping notification for user {UserId} as notifications are disabled",
                user.UserId
            );
            return;
        }

        await messaging.SendAsync(message);
    }

    public Task DeleteScheduledJobs(Guid userId, Guid GoalId, List<ReadingSessionResponse> sessions)
    {
        if (!sessions.Any())
            return Task.CompletedTask;

        var monitoring = JobStorage.Current.GetMonitoringApi();
        var scheduled = monitoring.ScheduledJobs(0, int.MaxValue);
        var conn = JobStorage.Current.GetConnection();

        foreach (var session in sessions)
        {
            // Create job key for this specific completed session
            string sessionJobKey = $"{userId:N}-{GoalId:N}-day{session.DayNumber}";

            // Find and delete any scheduled jobs for this session
            foreach (var kv in scheduled)
            {
                var jobId = kv.Key;
                var jobKey = conn.GetJobParameter(jobId, "JobKey");

                if (jobKey == sessionJobKey)
                {
                    jobs.Delete(jobId);
                    log.LogInformation(
                        "Deleted job {JobId} for completed session day {Day} ({JobKey})",
                        jobId,
                        session.DayNumber,
                        jobKey
                    );
                }
            }
        }

        return Task.CompletedTask;
    }

    public async Task RegisterDeviceAsync(Guid userId, string deviceToken)
    {
        var user =
            await userRepository.FindOne(u => u.UserId == userId)
            ?? throw new GlobalErrorThrower(404, "User Not Found", "User not found");
        user.DeviceToken = deviceToken;
        user.EnableNotifications = true;
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        log.LogInformation("Notifications {Status} for user {UserId}", "enabled", userId);
    }

    public async Task ToggleNotificationsAsync(Guid userId, bool enable)
    {
        var user =
            await userRepository.FindOne(u => u.UserId == userId)
            ?? throw new GlobalErrorThrower(404, "User Not Found", "User not found");
        if (string.IsNullOrEmpty(user.DeviceToken))
            throw new GlobalErrorThrower(
                400,
                "Device Not Registered",
                "Please register your device first"
            );

        user.EnableNotifications = enable;
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        log.LogInformation(
            "Notifications {Status} for user {UserId}",
            enable ? "enabled" : "disabled",
            userId
        );
    }

    public async Task AddNotificationAsync(
        Guid userId,
        string title,
        string message,
        DateTime? scheduledTime = null
    )
    {
        var user =
            await userRepository.FindOne(u => u.UserId == userId)
            ?? throw new GlobalErrorThrower(404, "User Not Found", "User not found");
        if (string.IsNullOrEmpty(user.DeviceToken))
            throw new GlobalErrorThrower(
                400,
                "Device Not Registered",
                "Please register your device first"
            );

        if (!user.EnableNotifications)
            throw new GlobalErrorThrower(
                400,
                "Notifications Disabled",
                "Please enable notifications first"
            );

        var notification = new Message
        {
            Token = user.DeviceToken,
            Notification = new Notification { Title = title, Body = message },
        };

        if (scheduledTime.HasValue)
        {
            var jobKey = $"{userId:N}-notification-{Guid.NewGuid():N}";
            BackgroundJob.Schedule(() => SendAsync(notification), scheduledTime.Value);
            log.LogInformation(
                "Scheduled notification for user {UserId} at {Time}",
                userId,
                scheduledTime
            );
        }
        else
        {
            await SendAsync(notification);
            log.LogInformation("Sent immediate notification to user {UserId}", userId);
        }
    }

    public async Task DeleteNotificationAsync(Guid userId, Guid notificationId)
    {
        var user =
            await userRepository.FindOne(u => u.UserId == userId)
            ?? throw new GlobalErrorThrower(404, "User Not Found", "User not found");
        var monitoring = JobStorage.Current.GetMonitoringApi();
        var scheduled = await Task.Run(() => monitoring.ScheduledJobs(0, int.MaxValue));
        var conn = JobStorage.Current.GetConnection();

        bool found = false;
        foreach (var kv in scheduled)
        {
            var jobId = kv.Key;
            var jobKey = await Task.Run(() => conn.GetJobParameter(jobId, "JobKey"));
            if (jobKey == $"{userId:N}-notification-{notificationId:N}")
            {
                BackgroundJob.Delete(jobId);
                log.LogInformation(
                    "Deleted notification {NotificationId} for user {UserId}",
                    notificationId,
                    userId
                );
                found = true;
                break;
            }
        }

        if (!found)
            throw new GlobalErrorThrower(
                404,
                "Notification Not Found",
                "The specified notification was not found"
            );
    }
}
