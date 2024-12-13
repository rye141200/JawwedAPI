using AutoMapper;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
namespace JawwedAPI.Core.Services;

public class BookmarkServices(IGenericRepository<Bookmark> bookmarkRepository, IMapper mapper, IGenericRepositoryMapped<Bookmark, BookmarkResponse> bookmarkMapper) : IBookmarkServices
{
    public async Task<BookmarkResponse?> AddBookmark(BookmarkAddRequest bookmarkAddRequest)
    {
        Bookmark bookmark = mapper.Map<Bookmark>(bookmarkAddRequest);

        //! 1) check if the bookmark is already exists:
        bool isBookmarkExists = await bookmarkRepository.FindOne(existsbookmark => existsbookmark.UserId == bookmark.UserId && existsbookmark.VerseKey == bookmark.VerseKey) != null;
        if (isBookmarkExists) return null;
        //! 2) if not create new bookmark and store it:
        await bookmarkRepository.Create(bookmark);
        await bookmarkRepository.SaveChangesAsync();
        return mapper.Map<BookmarkResponse>(bookmark);

    }

    public async Task<bool> DeleteBookmark(int userId, string verseKey)
    {
        //! 1) check if the userId and verseKey exists
        Bookmark? deletedBookmark = await bookmarkRepository.FindOne(bookmark => bookmark.UserId == userId && bookmark.VerseKey == verseKey);
        if (deletedBookmark == null) return false;

        bookmarkRepository.Delete(deletedBookmark);
        await bookmarkRepository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<BookmarkResponse>> GetAllBookmarks(int userId)
    {
        return await bookmarkMapper.Find(Bookmark => Bookmark.UserId == userId);
    }
}
