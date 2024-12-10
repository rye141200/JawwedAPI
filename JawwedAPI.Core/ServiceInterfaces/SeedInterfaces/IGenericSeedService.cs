namespace JawwedAPI.Core.ServiceInterfaces.SeedInterfaces;

/// <summary>
/// Defines a service for seeding data from json files to database entities
/// </summary>
/// <typeparam name="T">JsonBindedClass</typeparam>
/// <typeparam name="M">Database entity</typeparam>
public interface IGenericSeedService<T, M> where T : class where M : class
{
    /// <summary>
    /// Seeds the data from JsonBindedClass to the database
    /// </summary>
    /// <returns>Task</returns>
    IEnumerable<M> Seed(string path);

    Task SaveToDatabase(string path);
}
