using System;

namespace JawwedAPI.Core.DTOs;

/// <summary>
/// Data transfer object for tafsir (Quranic commentary) requests.
/// Contains the parameters necessary to retrieve specific tafsir content.
/// </summary>
/// <remarks>
/// This class is used to pass tafsir request parameters between API controllers and services.
/// The parameters allow filtering tafsir content by scholar, chapter/verse, and language.
/// </remarks>
public class TafsirRequest
{
    /// <summary>
    /// The name or identifier of the mofasir (scholar/interpreter) whose tafsir is being requested.
    /// </summary>
    /// <example>ibn-kathir, tabari, qurtubi</example>
    public required string MofasirID { get; set; }

    /// <summary>
    /// The chapter number when requesting chapter tafsir,
    /// or the chapter:verse format (e.g. "1:1") when requesting a specific verse tafsir.
    /// </summary>
    /// <example>"1" for chapter 1, or "1:1" for chapter 1, verse 1</example>
    public required string Chapter { get; set; }

    /// <summary>
    /// The language in which to return the tafsir content.
    /// Defaults to Arabic if not specified.
    /// </summary>
    /// <example>العربية, English, Français</example>
    public string? Language { get; set; } = "العربية";
}
