using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using JawwedAPI.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class MushafController(IMushafServices mushafServices) : CustomBaseController
{
    /// <summary>
    /// Gets all lines on a specific Mushaf page
    /// </summary>
    /// <param name="pageNumber">Page number (1-604)</param>
    /// <response code="200">Returns the page content</response>
    /// <response code="400">If page number is invalid</response>
    [HttpGet("pages/{pageNumber}")]
    [ProducesResponseType(typeof(IEnumerable<LineResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPage([FromRoute] int pageNumber)
    {
        IEnumerable<LineResponse> pageWithLines = await mushafServices.GetMushafPage(pageNumber);
        return Ok(pageWithLines);
    }

    /// <summary>
    /// Gets a specific verse by chapter and verse number
    /// </summary>
    /// <param name="chapterNumber">Chapter number (1-114)</param>
    /// <param name="verseNumber">Verse number within the chapter</param>
    /// <response code="200">Returns the requested verse</response>
    /// <response code="400">If chapter or verse number is invalid</response>
    [HttpGet("chapters/{chapterNumber}/verses/{verseNumber}")]
    [ProducesResponseType(typeof(VerseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetVerse(int chapterNumber, int verseNumber)
    {
        VerseResponse verse = await mushafServices.GetVerseByChapterNumberAndVerseNumber(
            chapterNumber,
            verseNumber
        );
        return Ok(verse);
    }

    /// <summary>
    /// Gets all verses in a specific chapter
    /// </summary>
    /// <param name="chapterNumber">Chapter number (1-114)</param>
    /// <response code="200">Returns all verses in the chapter</response>
    /// <response code="400">If chapter number is invalid</response>
    [HttpGet("chapters/{chapterNumber}/verses")]
    [ProducesResponseType(typeof(ChapterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetVerses(int chapterNumber)
    {
        ChapterResponse verses = await mushafServices.GetVersesByChapterNumber(chapterNumber);
        return Ok(verses);
    }

    /// <summary>
    /// Gets metadata for a specific chapter
    /// </summary>
    /// <param name="chapterNumber">Chapter number (1-114)</param>
    /// <response code="200">Returns the chapter metadata</response>
    /// <response code="400">If chapter number is invalid</response>
    [HttpGet("chapters/{chapterNumber}")]
    [ProducesResponseType(typeof(ChapterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetChapterInfo(int chapterNumber)
    {
        ChapterResponse chapter = await mushafServices.GetChapterMetaData(chapterNumber);
        return Ok(chapter);
    }
}
