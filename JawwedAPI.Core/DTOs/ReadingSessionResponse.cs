using System;
using JawwedAPI.Core.Domain.Enums;

namespace JawwedAPI.Core.DTOs;

/// <summary>
/// Represents a single reading session within a Quran reading goal
/// </summary>
public class ReadingSessionResponse
{
    /// <summary>
    /// Unique identifier for the reading session
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Sequential day number within the reading plan (1-based)
    /// </summary>
    public int DayNumber { get; set; }

    /// <summary>
    /// Date when this reading session is scheduled to occur
    /// </summary>
    public DateTimeOffset ScheduledDate { get; set; }

    /// <summary>
    /// First Quran page to read in this session
    /// </summary>
    public int StartPage { get; set; }

    /// <summary>
    /// Last Quran page to read in this session
    /// </summary>
    public int EndPage { get; set; }

    /// <summary>
    /// Actual number of pages read by the user, if session has been started or completed
    /// </summary>
    public int ActualPagesRead { get; set; } = 0;

    /// <summary>
    /// Current status of the reading session
    /// </summary>
    public string Status { get; set; } = ReadingSessionStatus.NotStarted.ToString();

    /// <summary>
    /// Total number of pages scheduled for this reading session
    /// </summary>
    public int PlannedPageCount => EndPage - StartPage + 1;
}
