using System;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;

namespace JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
/// <summary>
/// Interface containing the signature methods for the tafsir service.
/// Provides functionality to retrieve tafsir texts and information related to Mofasirs.
/// </summary>
public interface ITafsirService
{
    /// <summary>
    /// Retrieves a list of tafsir texts for the specified chapter or verse using the provided Mofasir's interpretation.
    /// </summary>
    /// <param name="mofasir">The Mofasir whose tafsir interpretation should be used.</param>
    /// <param name="ChapterVerseID">The identifier of the chapter or verse for which to obtain tafsir texts.</param>
    /// <returns>A list of tafsir texts as strings associated with the specified chapter/verse.</returns>
    public Task<TafsirResponse> GetVerseTafsir(string? mofasirName, string? ChapterVerseID, string? language = "العربية");
    /// <summary>
    /// Retrieves a list of tafsir texts for the specified chapter using the provided Mofasir's interpretation.
    /// </summary>
    /// <param name="mofasir">The Mofasir whose tafsir interpretation should be used.</param>
    /// <param name="Chapter">The identifier of the chapter for which to obtain tafsir texts.</param>
    /// <returns>
    /// A list of tafsir texts as strings associated with the specified chapter.
    /// </returns>
    public Task<List<TafsirResponse>> GetChapterTafsir(string? mofasirName, string? Chapter, string? language = "العربية");

}