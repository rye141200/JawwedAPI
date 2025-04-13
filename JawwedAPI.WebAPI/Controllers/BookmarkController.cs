using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BookmarkController(IBookmarkServices bookmarkService) : CustomBaseController
{
    private Guid? GetUserID()
    {
        var userIDClaim = User.Claims.FirstOrDefault(claim =>
            claim.Type == ClaimTypes.NameIdentifier
        );

        if (userIDClaim == null || string.IsNullOrEmpty(userIDClaim.Value))
            return null;

        if (Guid.TryParse(userIDClaim.Value, out Guid userID))
            return userID;

        return null;
    }

    [HttpPost]
    public async Task<IActionResult> AddBookmark(BookmarkAddRequest bookmarkAddRequest)
    {
        Guid? userID = GetUserID();
        if (userID == null)
            return NotFound("User ID does not exist!");

        bookmarkAddRequest.UserId = userID.Value;
        BookmarkResponse? bookmark = await bookmarkService.AddBookmark(bookmarkAddRequest);
        return Ok(bookmark);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBookmark(string verseKey)
    {
        Guid? userID = GetUserID();
        if (userID == null)
            return NotFound("User ID does not exist!");

        await bookmarkService.DeleteBookmark(userID.Value, verseKey);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetBookmarks()
    {
        Guid? userID = GetUserID();
        if (userID == null)
            return NotFound("User ID does not exist!");

        return Ok(await bookmarkService.GetAllBookmarks(userID.Value));
    }
}
