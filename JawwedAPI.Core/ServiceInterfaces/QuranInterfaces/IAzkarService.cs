using JawwedAPI.Core.DTOs;

namespace JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;

/// <summary>
/// Provides services for managing bookmarks from get all bookmarks, update some and delete.
/// </summary>
public interface IAzkarService
{
    Task<List<ZekrCategoryResponse>> GetAzkarCategories();
    Task<ZekrResponse> GetZekrById(int categoryId);
}
