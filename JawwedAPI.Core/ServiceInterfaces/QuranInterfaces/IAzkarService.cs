using JawwedAPI.Core.DTOs;

namespace JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;

/// <summary>
/// Provides services for accessing and retrieving Azkar data.
/// </summary>
public interface IAzkarService
{
    /// <summary>
    /// Retrieves all unique Azkar categories from the database.
    /// </summary>
    /// <returns>A list of Azkar category objects containing category ID, name, and audio.</returns>
    /// <exception cref="GlobalErrorThrower">Thrown when a server error occurs during data retrieval.</exception>
    Task<List<ZekrCategoryResponse>> GetAzkarCategories();

    /// <summary>
    /// Retrieves all Azkar items belonging to a specific category.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the Azkar category.</param>
    /// <returns>A ZekrResponse object containing the category information and a collection of all Azkar items in that category.</returns>
    /// <exception cref="GlobalErrorThrower">Thrown when the category doesn't exist, parameters are invalid, or a server error occurs.</exception>
    Task<ZekrResponse> GetZekrById(int categoryId);

    /// <summary>
    /// Retrieves a random Zekr item from any category in the database.
    /// </summary>
    /// <returns>A randomly selected Zekr item with its complete metadata.</returns>
    /// <exception cref="GlobalErrorThrower">Thrown when no Azkar items exist in the database or a server error occurs.</exception>
    Task<RandomZekrResponse> GetRandomZekr();

    /// <summary>
    /// Retrieves a random Zekr item from a specified category.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the Azkar category to select from.</param>
    /// <returns>A randomly selected Zekr item from the specified category with its complete metadata.</returns>
    /// <exception cref="GlobalErrorThrower">Thrown when the category doesn't exist, contains no items, the parameters are invalid, or a server error occurs.</exception>
    Task<RandomZekrResponse> GetRandomZekrFromCategory(int categoryId);
}
