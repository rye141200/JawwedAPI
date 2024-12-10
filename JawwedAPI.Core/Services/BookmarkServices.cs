using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
namespace JawwedAPI.Core.Services;

public class BookmarkServices : IBookmarkServices
{
    public Task AddBookmark(BookmarkAddRequest bookmarkAddRequest)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBookmark(int userId, string verseKey)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BookmarkResponse>> GetAllBookmarks(int userId)
    {
        throw new NotImplementedException();
    }
}
