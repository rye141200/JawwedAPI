using System.ComponentModel.DataAnnotations;
using AutoMapper;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.Enums;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Exceptions;
using JawwedAPI.Core.Exceptions.CustomExceptions;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using Microsoft.Extensions.Configuration;

namespace JawwedAPI.Core.Services;

public class BookmarkServices(
    IGenericRepository<Bookmark> bookmarkRepository,
    IMapper mapper,
    IGenericRepositoryMapped<Bookmark, BookmarkResponse> bookmarkMapper,
    IGenericRepository<ApplicationUser> userRepository,
    IGenericRepository<Zekr> zekrRepository,
    IConfiguration configuration
) : IBookmarkServices
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
        if (bookmarkAddRequest.UserId == null)
            throw new GlobalErrorThrower(400, "Invalid Data", "UserId is required");

        if (await userRepository.GetOne(bookmarkAddRequest.UserId.Value) == null)
            throw new GlobalErrorThrower(
                404,
                "Invalid Data",
                $"The user ID :{bookmarkAddRequest.UserId} is not found"
            );

        //! 2) check if the new bookmark already exists in the databse
        Bookmark? existingBookmark = null;
        if (bookmarkAddRequest.BookmarkType == BookmarkType.Quran)
        {
            existingBookmark = await bookmarkRepository.FindOne(bookmark =>
                bookmark.UserId == bookmarkAddRequest.UserId
                && bookmark.VerseKey == bookmarkAddRequest.VerseKey
            );
        }
        else if (bookmarkAddRequest.BookmarkType == BookmarkType.Zekr)
        {
            existingBookmark = await bookmarkRepository.FindOne(bookmark =>
                bookmark.UserId == bookmarkAddRequest.UserId
                && bookmark.ZekrID == bookmarkAddRequest.ZekrID
            );
        }
        if (existingBookmark != null)
        {
            throw new GlobalErrorThrower(
                409,
                "Already existing bookmark !",
                "The data you try to add already exists!"
            );
        }
        //! 3) if the type of bookmark is Quran
        if (bookmarkAddRequest.BookmarkType == BookmarkType.Quran)
        {
            //! 3.1) check if the verseKey is correct
            if (string.IsNullOrEmpty(bookmarkAddRequest.VerseKey))
                throw new GlobalErrorThrower(
                    400,
                    "Invalid Data",
                    "VerseKey is required for Quran bookmarks"
                );

            string[] verseParts = bookmarkAddRequest.VerseKey.Split(':');
            if (int.Parse(verseParts[0]) > 286 || int.Parse(verseParts[1]) > 114)
                throw new GlobalErrorThrower(
                    400,
                    "Invalid Data",
                    $"The entered Value :{bookmarkAddRequest.VerseKey} is invalid"
                );

            //!  3.1) Create and save new bookmark
            var bookmark = mapper.Map<Bookmark>(bookmarkAddRequest);
            await bookmarkRepository.Create(bookmark);
            await bookmarkRepository.SaveChangesAsync();
            return mapper.Map<BookmarkResponse>(bookmark);
        }
        else if (bookmarkAddRequest.BookmarkType == BookmarkType.Zekr)
        {
            //! 4) if the type of bookmark is Zekr

            //! 4.1) simple validation
            if (bookmarkAddRequest.ZekrID == null)
                throw new GlobalErrorThrower(
                    400,
                    "Invalid Data",
                    "ZekrID is required for zekr bookmarks"
                );
            if (await zekrRepository.GetOne(bookmarkAddRequest.ZekrID!.Value) == null)
                throw new GlobalErrorThrower(
                    400,
                    "Invalid Data",
                    "Invalid zekr data please try again"
                );
            //! 4.2) create and save new bookmark
            var bookmark = mapper.Map<Bookmark>(bookmarkAddRequest);
            await bookmarkRepository.Create(bookmark);
            await bookmarkRepository.SaveChangesAsync();
            return mapper.Map<BookmarkResponse>(bookmark);
        }
        throw new GlobalErrorThrower(
            500,
            "Invalid BookmarkType",
            "Please choose valid bookmark type"
        );
    }

    /// <summary>
    /// Deletes a user's bookmark
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="identifier">The identifier of the bookmark</param>
    /// <param name="type">The type of the bookmark</param>
    /// <returns>True if the bookmark was deleted, false otherwise</returns>
    /// <exception cref="GlobalErrorThrower">Thrown when the user or bookmark is not found</exception>
    public async Task<bool> DeleteBookmark(Guid userId, string identifier, BookmarkType type)
    {
        if (await userRepository.GetOne(userId) == null)
            throw new GlobalErrorThrower(404, "Invalid Data", $"User ID {userId} not found");

        Bookmark? bookmark =
            type switch
            {
                BookmarkType.Quran => await bookmarkRepository.FindOne(b =>
                    b.UserId == userId && b.VerseKey == identifier
                ),
                BookmarkType.Zekr => await bookmarkRepository.FindOne(b =>
                    b.UserId == userId && b.ZekrID == int.Parse(identifier)
                ),
                _ => throw new GlobalErrorThrower(400, "Invalid Type", "Invalid bookmark type"),
            } ?? throw new GlobalErrorThrower(404, "Not Found", "Bookmark not found");

        bookmarkRepository.Delete(bookmark);
        await bookmarkRepository.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Retrieves all bookmarks for a user
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <returns>Collection of bookmark responses</returns>
    /// <exception cref="GlobalErrorThrower">Thrown when the user is not found</exception>
    public async Task<IEnumerable<BookmarkResponse>> GetAllBookmarks(Guid userId)
    {
        if (await userRepository.GetOne(userId) == null)
            throw new GlobalErrorThrower(404, "Invalid Data", $"User ID {userId} not found");

        return (
                await bookmarkMapper.GetAllMappedPopulated(
                    bookmark => bookmark.UserId == userId,
                    bookmark => bookmark.Zekr!
                )
            ).Select(bookmarkresponse =>
            {
                if (bookmarkresponse.BookmarkType == BookmarkType.Quran)
                    return bookmarkresponse;
                string baselink = configuration["AudioAssets:AzkarAudioBaseLink"]!;
                Zekr zekr = bookmarkresponse.Zekr!;
                zekr.Audio = $"{baselink}/{zekr.Audio}";
                zekr.CategoryAudio = $"{baselink}/{zekr.CategoryAudio}";
                bookmarkresponse.Zekr = zekr;
                return bookmarkresponse;
            }) ?? [];
    }
}
