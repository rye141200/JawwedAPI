using FirebaseAdmin.Messaging;
using Hangfire;
using JawwedAPI.Core.Domain.Enums;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.NotificationInterfaces;
using Microsoft.Extensions.Logging;

namespace JawwedAPI.Core.Services;

public class FcmNotificationService(
    FirebaseMessaging messaging,
    IBackgroundJobClient jobs,
    ILogger<FcmNotificationService> log
) : INotificationService
{
    public Task SendAsync(Message message)
    {
        return messaging.SendAsync(message);
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
}
