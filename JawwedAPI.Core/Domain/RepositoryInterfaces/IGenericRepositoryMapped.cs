using System.Linq.Expressions;
namespace JawwedAPI.Core.Domain.RepositoryInterfaces;


public interface IGenericRepositoryMapped<T, M> where T : class where M : class
{
    Task<M?> FindOneMapped(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<M>> GetAllMapped();
}
