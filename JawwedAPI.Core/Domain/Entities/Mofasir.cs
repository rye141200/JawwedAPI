using System;
using System.ComponentModel.DataAnnotations;

namespace JawwedAPI.Core.Domain.Entities;

public class Mofasir
{
    [Key]
    public int MofasirID { get; set; }

    public string? BookName { get; set; }
    public string? AuthorName { get; set; }

    public string? Languages { get; set; }
    public virtual List<Tafsir> Tafsirs { get; set; } = new List<Tafsir>();
}
