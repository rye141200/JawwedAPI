using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace JawwedAPI.Core.Domain.Entities;

public class Verse
{
    [Key]
    [Required]
    public int VerseID { get; set; }

    [Required]
    [Range(1, 286)]
    public int VerseNumber { get; set; }


    [Required]
    [Range(1, 30)]
    public int JuzNumber { get; set; }

    [Required]
    [Range(1, 60)]
    public int HizbNumber { get; set; }

    [Required]
    [Range(1, 604)]
    public int Page { get; set; }

    [Required]
    public bool? Sajdah { get; set; }

    [Required]
    public string? TextUthmani { get; set; }

    //! Foreign => Chapter
    [ForeignKey("Chapter")]
    public int ChapterID { get; set; }


    //! Navigational property
    public virtual Chapter? Chapter { get; set; }
}
