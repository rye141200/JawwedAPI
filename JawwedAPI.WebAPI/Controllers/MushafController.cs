using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using JawwedAPI.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MushafController(IMushafServices mushafServices) : ControllerBase
    {
        [HttpGet("pages/{pageNumber}")]
        public async Task<IActionResult> GetPage([FromRoute] int pageNumber)
        {
            IEnumerable<LineResponse> pageWithLines = await mushafServices.GetMushafPage(pageNumber);
            return Ok(pageWithLines);
        }
        [HttpGet("chapters/{chapterNumber}/verses/{verseNumber}")]
        public async Task<IActionResult> GetVerse(int chapterNumber, int verseNumber)
        {
            VerseResponse verse = await mushafServices.GetVerseByChapterNumberAndVerseNumber(chapterNumber, verseNumber);
            return Ok(verse);
        }
        [HttpGet("chapters/{chapterNumber}/verses")]
        public async Task<IActionResult> GetVerses(int chapterNumber)
        {
            ChapterResponse verses = await mushafServices.GetVersesByChapterNumber(chapterNumber);
            return Ok(verses);
        }
        [HttpGet("chapters/{chapterNumber}")]
        public async Task<IActionResult> GetChapterInfo(int chapterNumber)
        {
            ChapterResponse chapter = await mushafServices.GetChapterMetaData(chapterNumber);
            return Ok(chapter);
        }
    }
}
