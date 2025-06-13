namespace JawwedAPI.Core.DTOs;

public class AddNotificationRequest
{
    public required string Title { get; set; }
    public required string Message { get; set; }
    public DateTime? ScheduledTime { get; set; }
}
