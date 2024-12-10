using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JawwedAPI.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
namespace JawwedAPI.Infrastructure.Repositories;

public class GenericRepositoryMapped<T, M>(ApplicationDbContext context, IMapper mapper) : IGenericRepositoryMapped<T, M> where T : class where M : class
{
    public async Task<M?> FindOneMapped(Expression<Func<T, bool>> predicate)
    => await context.Set<T>().
        Where(predicate).
        ProjectTo<M>(mapper.ConfigurationProvider).
        SingleOrDefaultAsync();
    public async Task<IEnumerable<M>> GetAllMapped()
    => await context.Set<T>().
    ProjectTo<M>(mapper.ConfigurationProvider).
    ToListAsync();
}
