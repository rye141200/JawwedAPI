namespace JawwedAPI.Core.Domain.Enums;

/// <summary>
/// Represents the current status of an individual reading session.
/// </summary>
public enum ReadingSessionStatus
{
    /// <summary>
    /// Reading session has not been started yet
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// Reading session is currently in progress
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Reading session was completed successfully
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Scheduled date passed without completion
    /// </summary>
    Missed = 3,
}
