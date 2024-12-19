
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;

namespace JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
public interface IMushafServices
{
    /// <summary>
    /// Get the page of the mushaf by the page number.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <returns>A list of LineResponse objects</returns>
    Task<IEnumerable<LineResponse>> GetMushafPage(int pageNumber);
    Task<VerseResponse> GetVerseByChapterNumberAndVerseNumber(int chapterNumber, int verseNumber);
    Task<ChapterResponse> GetVersesByChapterNumber(int chapterNumber);
    Task<ChapterResponse> GetChapterMetaData(int chapterNumber);
}
