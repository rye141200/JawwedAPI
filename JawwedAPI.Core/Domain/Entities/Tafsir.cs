using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JawwedAPI.Core.Domain.Entities;

public class Tafsir
{
    [Key]
    public int TafsirID { get; set; }
    public required string ChapterVerseID { get; set; }

    public required string Text { get; set; }

    [ForeignKey(nameof(Mofasir))]
    public int MofasirID { get; set; }

    public virtual Mofasir? Mofasir { get; set; }
}
