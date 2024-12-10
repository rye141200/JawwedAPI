using JawwedAPI.Infrastructure.DbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using JawwedAPI.Core.Domain.RepositoryInterfaces;

namespace JawwedAPI.Infrastructure.Repositories;

public class GenericProcedureRepository<T, M>(ApplicationDbContext context) : IGenericProcedureRepository<T, M> where T : class where M : class
{
    public async Task<IEnumerable<M>> ExecuteStoredProcedure(int pageNumber, string procedureName, string procedureParameterName)
    {
        return await context.Database.SqlQueryRaw<M>($"EXEC {procedureName} @{procedureParameterName}",
        new SqlParameter(procedureParameterName, pageNumber)).Select(obj => obj)
        .ToListAsync();
    }
}
