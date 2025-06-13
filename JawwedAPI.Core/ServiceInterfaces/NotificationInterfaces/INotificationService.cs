using System;
using FirebaseAdmin.Messaging;
using JawwedAPI.Core.DTOs;

namespace JawwedAPI.Core.ServiceInterfaces.NotificationInterfaces;

public interface INotificationService
{
    Task SendAsync(Message message);

    public Task DeleteScheduledJobs(
        Guid userId,
        Guid GoalId,
        List<ReadingSessionResponse> sessions
    );

    Task RegisterDeviceAsync(Guid userId, string deviceToken);

    Task ToggleNotificationsAsync(Guid userId, bool enable);

    Task AddNotificationAsync(
        Guid userId,
        string title,
        string message,
        DateTime? scheduledTime = null
    );

    Task DeleteNotificationAsync(Guid userId, Guid notificationId);
}
