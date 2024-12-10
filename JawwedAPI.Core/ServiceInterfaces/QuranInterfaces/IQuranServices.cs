
namespace JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
public interface IQuranServices<T>
{
    public Task<T?> GetOne(int id);
    public Task<IEnumerable<T>> GetAll();
}