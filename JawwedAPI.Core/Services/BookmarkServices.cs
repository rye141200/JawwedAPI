using System.ComponentModel.DataAnnotations;
using AutoMapper;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Exceptions;
using JawwedAPI.Core.Exceptions.CustomExceptions;
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
        //! 1) check if the data is correct 
        if (bookmarkAddRequest.UserId <= 0)
        {
            throw new GlobalErrorThrower(400, "Invalid Data", $"The entered Value :{bookmarkAddRequest.UserId} is invalid");
        }

        if (string.IsNullOrWhiteSpace(bookmarkAddRequest.VerseKey))
        {
            throw new GlobalErrorThrower(400, "Invalid Data", $"The entered Value :{bookmarkAddRequest.UserId} is invalid");

        }
        //! 2) check if the new bookmark already exists in the databse
        Bookmark? existingBookmark = await bookmarkRepository.FindOne(
            bookmark => bookmark.UserId == bookmarkAddRequest.UserId &&
                       bookmark.VerseKey == bookmarkAddRequest.VerseKey);

        if (existingBookmark != null)
        {
            throw new GlobalErrorThrower(409, "Already existing bookmark !", "The data you try to add already exists!");
        }
        //! 3) check if the verseKey is correct
        string[] verseParts = bookmarkAddRequest.VerseKey
                .Split(':');
        if (int.Parse(verseParts[0]) > 286 || int.Parse(verseParts[1]) > 114) throw new GlobalErrorThrower(400, "Invalid Data", $"The entered Value :{bookmarkAddRequest.VerseKey} is invalid");

        //!  4) Create and save new bookmark
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
        //! 1) check if the data is correct 
        if (userId <= 0)
        {
            throw new GlobalErrorThrower(400, "Invalid Data", $"The entered Value :{userId} is invalid");
        }

        if (string.IsNullOrWhiteSpace(verseKey))
        {
            throw new GlobalErrorThrower(400, "Invalid Data", $"The entered Value :{verseKey} is invalid");

        }
        //! 2) check if the bookmark that will be deleted is already exists in the databse
        Bookmark? bookmark = await bookmarkRepository.FindOne(
                                    bookmark => bookmark.UserId == userId &&
                                    bookmark.VerseKey == verseKey);

        if (bookmark is null) throw new GlobalErrorThrower(404, "Data not Found", "This bookmark is not exists in your bookmark collection");

        //! 3) delete the bookmark from the database
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
        //! 1) //! 1) check if the data is correct 
        if (userId <= 0)
        {
            throw new GlobalErrorThrower(400, "Invalid User ID", $"The entered user Identity {userId} is invalid");
        }
        //! 2) check if the entered userId is exists
        //todo check if the this user exists in the database

        //! 3) get all available bookmarks
        return await bookmarkMapper.Find(bookmark => bookmark.UserId == userId);
    }
}

