using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookmarkController(IBookmarkServices bookmarkService) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> AddBookmark(BookmarkAddRequest bookmarkAddRequest)
    {
        BookmarkResponse? bookmark = await bookmarkService.AddBookmark(bookmarkAddRequest);
        if (bookmark is null) return Conflict("This bookmark already exists.");
        return Ok(bookmark);
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteBookmark(int userId, string verseKey)
    {

        bool status = await bookmarkService.DeleteBookmark(userId, verseKey);
        if (status)
            //todo change to deleted status code
            return Ok();
        return NotFound();
    }
    public async Task<IActionResult> GetBookmarks(int userId)
    {
        return Ok(await bookmarkService.GetAllBookmarks(userId));
    }
}

