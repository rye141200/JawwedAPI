using JawwedAPI.Core.Domain.Enums;
using JawwedAPI.Core.DTOs;

namespace JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;

/// <summary>
/// Provides services for managing bookmarks from get all bookmarks, update some and delete.
/// </summary>
public interface IBookmarkServices
{
    /// <summary>
    /// Return list of all bookmarks.
    /// </summary>
    /// <returns>A list of bookmarkResponse objects.</returns>
    /// <param name="userId">the logged in userId</param>
    Task<IEnumerable<BookmarkResponse>> GetAllBookmarks(Guid userId);

    /// <summary>
    /// Adds a new bookmark.
    /// </summary>
    /// <param name="bookmark">The bookmarkAddRequest object.</param>
    Task<BookmarkResponse?> AddBookmark(BookmarkAddRequest bookmarkAddRequest);

    /// <summary>
    /// delete a specific bookmark from database
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="verseKey"></param>
    /// <returns></returns>
    public Task<bool> DeleteBookmark(Guid userId, string identifier, BookmarkType type);
}
