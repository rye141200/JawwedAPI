using System.ComponentModel.DataAnnotations;

namespace JawwedAPI.Core.Domain.Entities;

public class Line
{
    [Key]
    public int LineID { get; set; }

    [Required]
    public int LineNumber { get; set; }

    [Required]
    public string? LineType { get; set; }

    [Required]
    public int SurahNumber { get; set; }

    [Required]
    public int PageNumber { get; set; }

    [Required]
    public string? Text { get; set; }

    [Required]
    public bool IsCentered { get; set; }

    public int? JuzNumber { get; set; }

    public int? HizbNumber { get; set; }

    public int? RubHizbNumber { get; set; }

    public string? VersesKeys { get; set; }
    //! Navigation fields
    //public List<Verse>? Verses { get; set; }
}
