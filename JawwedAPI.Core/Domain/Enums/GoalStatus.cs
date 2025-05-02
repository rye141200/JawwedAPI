namespace JawwedAPI.Core.Domain.Enums;

/// <summary>
/// Represents the current status of a user's goal.
/// </summary>
public enum GoalStatus
{
    /// <summary>
    /// Goal is actively being worked on and is on track
    /// </summary>
    InProgress = 0,

    /// <summary>
    /// Goal has been successfully completed
    /// </summary>
    Completed = 1,

    /// <summary>
    /// Goal was canceled before completion
    /// </summary>
    Canceled = 2,
}
