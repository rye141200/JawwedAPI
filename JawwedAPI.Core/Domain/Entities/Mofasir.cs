using System;
using System.ComponentModel.DataAnnotations;

namespace JawwedAPI.Core.Domain.Entities;

public class Mofasir
{
    [Key]
    public int MofasirID { get; set; }

    // Book name in both languages
    public required string BookNameArabic { get; set; }
    public required string BookNameEnglish { get; set; }

    // Author name in both languages
    public required string AuthorNameArabic { get; set; }
    public required string AuthorNameEnglish { get; set; }

    // Language support flags
    public bool SupportsArabic { get; set; } = true;
    public bool SupportsEnglish { get; set; } = false;

    // Optional: Birth/Death dates for the scholar
    public string? BirthYear { get; set; }
    public string? DeathYear { get; set; }

    // Optional: Brief biography in both languages
    public string? BiographyArabic { get; set; }
    public string? BiographyEnglish { get; set; }

    // Navigation property
    public virtual List<Tafsir> Tafsirs { get; set; } = new List<Tafsir>();
}
