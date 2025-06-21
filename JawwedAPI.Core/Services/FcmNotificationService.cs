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
        try
        {
            // Get the user by device token
            var user = await userRepository.FindOne(u => u.DeviceToken == message.Token);
            if (user == null)
            {
                log.LogWarning("User not found for device token {Token}", message.Token);
                return; // Don't throw, just skip sending
            }

            // Only send if notifications are enabled
            if (!user.EnableNotifications)
            {
                log.LogInformation(
                    "Skipping notification for user {UserId} as notifications are disabled",
                    user.UserId
                );
                return;
            }

            // Send the FCM message
            var response = await messaging.SendAsync(message);
            log.LogInformation(
                "Successfully sent FCM notification to user {UserId}, message ID: {MessageId}",
                user.UserId,
                response
            );
        }
        catch (FirebaseMessagingException ex)
        {
            log.LogError(
                ex,
                "FCM error for token {Token}: {ErrorCode}",
                message.Token,
                ex.MessagingErrorCode
            );

            // Handle specific FCM errors
            switch (ex.MessagingErrorCode)
            {
                case MessagingErrorCode.Unregistered:
                case MessagingErrorCode.InvalidArgument:
                    // Token is invalid, remove it from user
                    await RemoveInvalidToken(message.Token);
                    break;

                case MessagingErrorCode.QuotaExceeded:
                    log.LogWarning("FCM quota exceeded for token {Token}", message.Token);
                    break;

                case MessagingErrorCode.SenderIdMismatch:
                    log.LogError("FCM sender ID mismatch for token {Token}", message.Token);
                    break;

                case MessagingErrorCode.ThirdPartyAuthError:
                    log.LogError("FCM third party auth error for token {Token}", message.Token);
                    break;

                default:
                    log.LogError(
                        "Unknown FCM error {ErrorCode} for token {Token}",
                        ex.MessagingErrorCode,
                        message.Token
                    );
                    break;
            }

            // Don't re-throw - let the calling code handle it
        }
        catch (Exception ex)
        {
            log.LogError(
                ex,
                "Unexpected error sending notification to token {Token}",
                message.Token
            );
            // Don't re-throw - let the calling code handle it
        }
    }

    private async Task RemoveInvalidToken(string token)
    {
        try
        {
            var user = await userRepository.FindOne(u => u.DeviceToken == token);
            if (user != null)
            {
                user.DeviceToken = string.Empty;
                user.EnableNotifications = false;
                userRepository.Update(user);
                await userRepository.SaveChangesAsync();
                log.LogWarning("Removed invalid FCM token for user {UserId}", user.UserId);
            }
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error removing invalid token {Token}", token);
        }
    }

    public Task DeleteScheduledJobs(Guid userId, Guid GoalId, List<ReadingSessionResponse> sessions)
    {
        if (!sessions.Any())
            return Task.CompletedTask;

        try
        {
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
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error deleting scheduled jobs for user {UserId}", userId);
        }

        return Task.CompletedTask;
    }

    public async Task RegisterDeviceAsync(Guid userId, string deviceToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(deviceToken))
                throw new GlobalErrorThrower(400, "Invalid Token", "Device token cannot be empty");

            // Basic FCM token validation - FCM tokens are typically 140+ characters
            if (deviceToken.Length < 100)
                throw new GlobalErrorThrower(400, "Invalid Token", "Invalid FCM token format");

            var user =
                await userRepository.FindOne(u => u.UserId == userId)
                ?? throw new GlobalErrorThrower(404, "User Not Found", "User not found");

            // Update the user's device token and enable notifications
            user.DeviceToken = deviceToken;
            user.EnableNotifications = true;
            userRepository.Update(user);
            await userRepository.SaveChangesAsync();

            log.LogInformation(
                "Device registered for user {UserId} with token length {TokenLength}",
                userId,
                deviceToken.Length
            );
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error registering device for user {UserId}", userId);
            throw;
        }
    }

    public async Task ToggleNotificationsAsync(Guid userId, bool enable)
    {
        try
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
        catch (Exception ex)
        {
            log.LogError(ex, "Error toggling notifications for user {UserId}", userId);
            throw;
        }
    }

    public async Task AddNotificationAsync(
        Guid userId,
        string title,
        string message,
        DateTime? scheduledTime = null
    )
    {
        try
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
        catch (Exception ex)
        {
            log.LogError(ex, "Error adding notification for user {UserId}", userId);
            throw;
        }
    }

    public async Task DeleteNotificationAsync(Guid userId, Guid notificationId)
    {
        try
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
        catch (Exception ex)
        {
            log.LogError(
                ex,
                "Error deleting notification {NotificationId} for user {UserId}",
                notificationId,
                userId
            );
            throw;
        }
    }
}
