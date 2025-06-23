using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JawwedAPI.Core.DTOs;

public class CreateGoalRequest : IValidatableObject
{
    /// <summary>
    /// User ID is automatically set from JWT token and should not be included in API requests
    /// </summary>
    [BindNever]
    [JsonIgnore] // Hides from Swagger/Scalar documentation and JSON serialization
    public Guid? UserId { get; set; }

    /// <summary>
    /// Title of the reading goal (e.g., "Ramadan Quran Completion", "Daily Reading Goal")
    /// </summary>
    [Required(ErrorMessage = "Goal title is required")]
    [StringLength(
        100,
        MinimumLength = 3,
        ErrorMessage = "Title must be between 3 and 100 characters"
    )]
    public required string Title { get; set; }

    /// <summary>
    /// Duration of the goal in days (minimum 1 day, maximum 365 days)
    /// </summary>
    [Required(ErrorMessage = "Duration in days is required")]
    public int DurationDays { get; set; }

    /// <summary>
    /// Total number of pages to read (minimum 1 page, maximum 604 pages - full Quran)
    /// </summary>
    [Required(ErrorMessage = "Total pages is required")]
    [Range(1, 604, ErrorMessage = "Total pages must be between 1 and 604 (full Quran)")]
    public int TotalPages { get; set; }

    /// <summary>
    /// Starting page number (minimum page 1, maximum page 604)
    /// </summary>
    [Required(ErrorMessage = "Start page is required")]
    [Range(1, 604, ErrorMessage = "Start page must be between 1 and 604")]
    public int StartPage { get; set; }

    /// <summary>
    /// Daily reminder time in TimeSpan format (e.g., "08:30:00" for 8:30 AM)
    /// </summary>
    [Required(ErrorMessage = "Reminder time is required")]
    [ValidReminderTime(
        ErrorMessage = "Reminder time must be a valid time within 24 hours (00:00:00 to 23:59:59)"
    )]
    public TimeSpan ReminderTime { get; set; }

    /// <summary>
    /// Custom validation to ensure the goal is realistic and achievable
    /// </summary>
    /// <returns>Validation result</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        // Validate that start page + total pages doesn't exceed Quran length
        if (StartPage + TotalPages - 1 > 604)
        {
            results.Add(
                new ValidationResult(
                    "The combination of start page and total pages exceeds the Quran length (604 pages)",
                    [nameof(StartPage), nameof(TotalPages)]
                )
            );
        }

        // Validate minimum pages per day (should be reasonable)
        var pagesPerDay = (double)TotalPages / DurationDays;
        if (pagesPerDay > 50)
        {
            results.Add(
                new ValidationResult(
                    $"Goal requires reading {pagesPerDay:F1} pages per day, which may be too ambitious. Consider increasing duration or reducing pages.",
                    [nameof(DurationDays), nameof(TotalPages)]
                )
            );
        }

        // Validate that reminder time is not in seconds precision (common user error)
        if (ReminderTime.Milliseconds != 0)
        {
            results.Add(
                new ValidationResult(
                    "Reminder time should not include milliseconds. Use format HH:mm:ss",
                    [nameof(ReminderTime)]
                )
            );
        }

        return results;
    }
}

/// <summary>
/// Custom validation attribute for reminder time
/// </summary>
public class ValidReminderTimeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not TimeSpan timeSpan)
            return new ValidationResult("Reminder time must be a valid TimeSpan");

        // Check if time is within 24 hours
        if (timeSpan < TimeSpan.Zero || timeSpan >= TimeSpan.FromDays(1))
            return new ValidationResult("Reminder time must be between 00:00:00 and 23:59:59");

        // Check if time has reasonable precision (no milliseconds)
        if (timeSpan.Milliseconds != 0)
            return new ValidationResult("Reminder time should not include milliseconds");

        return ValidationResult.Success;
    }
}
