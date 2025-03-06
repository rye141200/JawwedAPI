using System.ComponentModel.DataAnnotations;

namespace JawwedAPI.Core.Domain.Entities;

public class Chapter
{
    [Key]
    public int ChapterID { get; set; }

    [Required]
    public string? RevelationPlace { get; set; }

    [Required]
    public Boolean BismallahPre { get; set; }

    [Required]
    public string? NameComplex { get; set; }

    [Required]
    public string? NameArabic { get; set; }

    [Required]
    public string? NameEnglish { get; set; }

    [Required]
    public string? Pages { get; set; }

    [Required]
    public int VersesCount { get; set; }

    //! Navigation fields

    public List<Verse>? Verses { get; set; }
}
