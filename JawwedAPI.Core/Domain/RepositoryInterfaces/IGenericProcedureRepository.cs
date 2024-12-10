namespace JawwedAPI.Core.Domain.RepositoryInterfaces;
/// <summary>
/// 
/// </summary>
/// <typeparam name="T">Base class where the procedure is performed</typeparam>
/// <typeparam name="M">Output DTO from the stored procedure</typeparam>
public interface IGenericProcedureRepository<T, M> where T : class where M : class
{
    public Task<IEnumerable<M>> ExecuteStoredProcedure(int pageNumber, string procedureName, string procedureParameterName);
}
/* 
    => await context.Set<T>()
        .FromSqlRaw($"EXEC {procedureName} @{procedureParameterName}", new SqlParameter(procedureParameterName, pageNumber))
        .ToListAsync();
 */