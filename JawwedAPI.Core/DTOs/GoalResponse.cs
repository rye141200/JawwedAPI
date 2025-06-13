using JawwedAPI.Core.DTOs;

public class GoalResponse
{
    public Guid GoalId { get; set; }
    public required string Title { get; init; }
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate => StartDate.AddDays(DurationDays);
    public string PageRange => $"{StartPage} - {EndPage}";
    public int TotalPages { get; init; }
    public int DurationDays { get; set; }
    public int ActualPagesRead { get; init; }
    public int ProgressPercent => (int)Math.Round(100m * ActualPagesRead / TotalPages);
    public int StartPage { get; init; }
    public int EndPage => StartPage + TotalPages - 1;
    public string LastVerseKeyRead { get; set; } = string.Empty;
    public required string Status { get; set; }
    public TimeSpan ReminderTime { get; set; }

    public List<ReadingSessionResponse> ReadingSchedule { get; set; } = [];
}
