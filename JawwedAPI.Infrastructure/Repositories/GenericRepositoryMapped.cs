using System.Linq.Expressions;
using AutoMapper;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace JawwedAPI.Infrastructure.Repositories;

public class GenericRepositoryMapped<T, M>(ApplicationDbContext context, IMapper mapper)
    : IGenericRepositoryMapped<T, M>
    where T : class
    where M : class
{
    public async Task<M?> FindOneMapped(Expression<Func<T, bool>> predicate) =>
        mapper.Map<M?>(await context.Set<T>().Where(predicate).SingleOrDefaultAsync());

    public async Task<IEnumerable<M>> GetAllMapped() =>
        mapper.Map<IEnumerable<M>>(await context.Set<T>().ToListAsync());

    public async Task<IEnumerable<M>> Find(Expression<Func<T, bool>> predicate) =>
        mapper.Map<IEnumerable<M>>(await context.Set<T>().Where(predicate).ToListAsync());

    public async Task<M?> FindOneMappedPopulated(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> includeExpression
    ) =>
        mapper.Map<M?>(
            await context
                .Set<T>()
                .Include(includeExpression)
                .Where(predicate)
                .SingleOrDefaultAsync()
        );

    public async Task<IEnumerable<M>> GetAllMappedPopulated(
        Expression<Func<T, object>> includeExpression
    ) =>
        mapper.Map<IEnumerable<M>>(await context.Set<T>().Include(includeExpression).ToListAsync());
}
