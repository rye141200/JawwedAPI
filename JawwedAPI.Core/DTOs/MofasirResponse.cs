using System;
using System.Collections.Generic;

namespace JawwedAPI.Core.DTOs;

/// <summary>
/// Data transfer object that represents a Mofasir (Quranic commentator) for API responses
/// </summary>
public class MofasirResponse
{
    /// <summary>
    /// Unique identifier for the Mofasir
    /// </summary>
    public int MofasirID { get; set; }

    /// <summary>
    /// The Arabic name of the scholar/author
    /// </summary>
    public required string AuthorNameArabic { get; set; }

    /// <summary>
    /// The English name of the scholar/author
    /// </summary>
    public required string AuthorNameEnglish { get; set; }

    /// <summary>
    /// The Arabic title of the Tafsir book
    /// </summary>
    public required string BookNameArabic { get; set; }

    /// <summary>
    /// The English title of the Tafsir book
    /// </summary>
    public required string BookNameEnglish { get; set; }

    /// <summary>
    /// Brief biography of the Mofasir in Arabic
    /// </summary>
    public required string BiographyArabic { get; set; }

    /// <summary>
    /// Brief biography of the Mofasir in English
    /// </summary>
    public required string BiographyEnglish { get; set; }

    /// <summary>
    /// Whether this Mofasir's works are available in Arabic
    /// </summary>
    public bool SupportsArabic { get; set; }

    /// <summary>
    /// Whether this Mofasir's works are available in English
    /// </summary>
    public bool SupportsEnglish { get; set; }

    /// <summary>
    /// Birth year of the Mofasir
    /// </summary>
    public required string BirthYear { get; set; }

    /// <summary>
    /// Death year of the Mofasir
    /// </summary>
    public required string DeathYear { get; set; }
}
