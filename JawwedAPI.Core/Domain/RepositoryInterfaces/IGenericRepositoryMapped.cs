using System.Linq.Expressions;

namespace JawwedAPI.Core.Domain.RepositoryInterfaces;

public interface IGenericRepositoryMapped<T, M>
    where T : class
    where M : class
{
    Task<M?> FindOneMapped(Expression<Func<T, bool>> predicate);
    Task<M?> FindOneMappedPopulated(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> includeExpression
    );
    Task<IEnumerable<M>> GetAllMapped();
    public Task<IEnumerable<M>> GetAllMappedPopulated(
        Expression<Func<T, bool>> filter,
        params Expression<Func<T, object>>[] includes
    );
    Task<IEnumerable<M>> Find(Expression<Func<T, bool>> predicate);
}
