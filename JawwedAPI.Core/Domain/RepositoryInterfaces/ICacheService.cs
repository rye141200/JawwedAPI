using System;

namespace JawwedAPI.Core.Domain.RepositoryInterfaces;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key)
        where T : class;
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        where T : class;
    Task<bool> RemoveAsync(string key);
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        where T : class;
}
