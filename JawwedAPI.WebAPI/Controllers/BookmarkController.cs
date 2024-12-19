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
        return Ok(bookmark);
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteBookmark(int userId, string verseKey)
    {

        await bookmarkService.DeleteBookmark(userId, verseKey);
        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> GetBookmarks(int userId)
    {
        return Ok(await bookmarkService.GetAllBookmarks(userId));
    }
}

