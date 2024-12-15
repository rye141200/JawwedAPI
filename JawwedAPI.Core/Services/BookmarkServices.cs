using System.ComponentModel.DataAnnotations;
using AutoMapper;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Exceptions;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
namespace JawwedAPI.Core.Services;


public class BookmarkServices(IGenericRepository<Bookmark> bookmarkRepository, IMapper mapper, IGenericRepositoryMapped<Bookmark, BookmarkResponse> bookmarkMapper) : IBookmarkServices
{
    /// <summary>
    /// Adds a new bookmark for a user
    /// </summary>
    /// <param name="bookmarkAddRequest">The bookmark details to add</param>
    /// <returns>The created bookmark response</returns>
    /// <exception cref="BookmarkExistsException">Thrown when bookmark already exists</exception>
    public async Task<BookmarkResponse?> AddBookmark(BookmarkAddRequest bookmarkAddRequest)
    {

        ValidateBookmarkRequest(bookmarkAddRequest);

        // Check for existing bookmark
        var existingBookmark = await FindExistingBookmark(
            bookmarkAddRequest.UserId,
            bookmarkAddRequest.VerseKey);

        if (existingBookmark != null)
        {
            return null;
        }

        // Create and save new bookmark
        var bookmark = mapper.Map<Bookmark>(bookmarkAddRequest);
        await bookmarkRepository.Create(bookmark);
        await bookmarkRepository.SaveChangesAsync();

        return mapper.Map<BookmarkResponse>(bookmark);
    }

    /// <summary>
    /// Deletes a user's bookmark
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="verseKey">The verse key to remove</param>
    public async Task<bool> DeleteBookmark(int userId, string verseKey)
    {
        ValidateDeleteRequest(userId, verseKey);

        Bookmark? bookmark = await FindExistingBookmark(userId, verseKey);

        if (bookmark is null) return false;

        bookmarkRepository.Delete(bookmark);
        await bookmarkRepository.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Retrieves all bookmarks for a user
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <returns>Collection of bookmark responses</returns>
    public async Task<IEnumerable<BookmarkResponse>> GetAllBookmarks(int userId)
    {
        if (userId <= 0)
        {
            throw new DataValidationException(400, "Invalid User ID", $"The entered user Identity {userId} is invalid");
        }

        return await bookmarkMapper.Find(bookmark => bookmark.UserId == userId);
    }

    private async Task<Bookmark?> FindExistingBookmark(int userId, string verseKey)
    {
        return await bookmarkRepository.FindOne(
            bookmark => bookmark.UserId == userId &&
                       bookmark.VerseKey == verseKey);
    }

    private static void ValidateBookmarkRequest(BookmarkAddRequest request)
    {
        if (request.UserId <= 0)
        {
            throw new DataValidationException(400, "Invalid User ID", $"The entered user Identity {request.UserId} is invalid");
        }

        if (string.IsNullOrWhiteSpace(request.VerseKey))
        {
            throw new DataValidationException(400, "Invalid Verse key", $"The entered Verse key {request.VerseKey} is invalid");
        }
    }

    private static void ValidateDeleteRequest(int userId, string verseKey)
    {
        if (userId <= 0)
        {
            throw new DataValidationException(400, "Invalid User ID", $"The entered user Identity {userId} is invalid");
        }

        if (string.IsNullOrWhiteSpace(verseKey))
        {
            throw new DataValidationException(400, "Invalid Verse key", $"The entered Verse key {verseKey} is invalid");
        }
    }
}

