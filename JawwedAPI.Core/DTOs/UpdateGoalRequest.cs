using System.ComponentModel.DataAnnotations;

public class UpdateGoalRequest
{
    [Required(ErrorMessage = "Last page not found")]
    [Range(0, 604)]
    public int LastPageRead { get; set; }

    [Required(ErrorMessage = "Last versekey read not found")]
    public required string LastVerseKeyRead { get; set; }
}
