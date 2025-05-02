using System;
using FirebaseAdmin.Messaging;
using JawwedAPI.Core.DTOs;

namespace JawwedAPI.Core.ServiceInterfaces.NotificationInterfaces;

public interface INotificationService
{
    Task SendAsync(Message message);

    /// <summary>
    /// Sync one‐off Hangfire jobs for this user:
    /// delete any job whose JobKey no longer corresponds to an InProgress session.
    /// </summary>
    public Task DeleteScheduledJobs(
        Guid userId,
        Guid GoalId,
        List<ReadingSessionResponse> sessions
    );
}
