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
    public string AuthorNameArabic { get; set; } = string.Empty;

    /// <summary>
    /// The English name of the scholar/author
    /// </summary>
    public string AuthorNameEnglish { get; set; } = string.Empty;

    /// <summary>
    /// The Arabic title of the Tafsir book
    /// </summary>
    public string BookNameArabic { get; set; } = string.Empty;

    /// <summary>
    /// The English title of the Tafsir book
    /// </summary>
    public string BookNameEnglish { get; set; } = string.Empty;

    /// <summary>
    /// Brief biography of the Mofasir in Arabic
    /// </summary>
    public string BiographyArabic { get; set; } = string.Empty;

    /// <summary>
    /// Brief biography of the Mofasir in English
    /// </summary>
    public string BiographyEnglish { get; set; } = string.Empty;

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
    public string BirthYear { get; set; } = string.Empty;

    /// <summary>
    /// Death year of the Mofasir
    /// </summary>
    public string DeathYear { get; set; } = string.Empty;
}