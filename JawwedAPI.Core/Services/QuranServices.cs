
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
namespace JawwedAPI.Core.Services;
// what da hell is even that????
public class QuranServices<T>(IGenericRepository<T> repository) : IQuranServices<T> where T : class
{

    public async Task<IEnumerable<T>> GetAll()
    {
        return await repository.GetAll();
    }

    public async Task<T?> GetOne(int id)
    {
        return await repository.GetOne(id);
    }
}
