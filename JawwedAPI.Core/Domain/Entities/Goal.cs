using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JawwedAPI.Core.Domain.Enums;

namespace JawwedAPI.Core.Domain.Entities;

public class Goal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid GoalId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Title { get; set; } = "Quran Khatma"; // Default title
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public int DurationDays { get; set; } = 30;

    [Range(1, 604, ErrorMessage = "This number of pages is not correct please choose another one")]
    public int StartPage { get; set; } = 1; // By default quran khatma
    public int TotalPages { get; set; } = 604; // By default quran khatma

    [Range(1, 604, ErrorMessage = "This number of pages is not correct please choose another one")]
    public int ActualPagesRead { get; set; } = 0;
    public GoalStatus Status { get; set; } = GoalStatus.InProgress;

    public string LastVerseKeyRead { get; set; } = string.Empty;

    //! navigational property
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }
}
