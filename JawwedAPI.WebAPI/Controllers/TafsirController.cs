using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.WebAPI.Controllers
{
    /// <summary>
    /// API controller for retrieving tafsir (Quranic commentary) content.
    /// Provides endpoints to access tafsir by chapter or individual verse.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TafsirController(ITafsirService tafsirService) : ControllerBase
    {
        /// <summary>
        /// Retrieves tafsir content for all verses in a specific chapter.
        /// </summary>
        /// <param name="tafsirRequest">Request data containing mofasir name, chapter number, and optional language</param>
        /// <returns>A collection of tafsir entries for all verses in the specified chapter</returns>
        [HttpGet("chapter")]
        public async Task<IActionResult> GetChapterTafsir([FromQuery] TafsirRequest tafsirRequest)
        {
            // Get all tafsir entries for the specified chapter
            List<TafsirResponse> tafsirs = await tafsirService.GetChapterTafsir(
                tafsirRequest.MofasirID,
                tafsirRequest.Chapter,
                tafsirRequest.Language
            );

            return Ok(tafsirs);
        }

        /// <summary>
        /// Retrieves tafsir content for a specific verse.
        /// </summary>
        /// <param name="tafsirRequest">Request data containing mofasir name, verse ID (format: chapter:verse), and optional language</param>
        /// <returns>The tafsir entry for the specified verse</returns>
        [HttpGet("verse")]
        public async Task<IActionResult> GetVerseTafsir([FromQuery] TafsirRequest tafsirRequest)
        {
            // Get tafsir entry for the specified verse
            TafsirResponse tafsir = await tafsirService.GetVerseTafsir(
                tafsirRequest.MofasirID,
                tafsirRequest.Chapter,
                tafsirRequest.Language
            );

            return Ok(tafsir);
        }
    }
}
