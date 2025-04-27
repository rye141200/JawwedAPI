using System;
using System.Threading.Tasks;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using Microsoft.Extensions.Caching.Memory;

namespace JawwedAPI.Infrastructure.Repositories;

public class MemoryCacheService(IMemoryCache _memoryCache) : ICacheService
{
    public Task<T?> GetAsync<T>(string key)
        where T : class
    {
        return Task.FromResult(_memoryCache.TryGetValue(key, out T? value) ? value : null);
    }

    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? expiration = null
    )
        where T : class
    {
        ArgumentNullException.ThrowIfNull(factory);

        //!1) Try retreiving from the cache
        var cachedValue = await GetAsync<T>(key);
        if (cachedValue != null)
            return cachedValue;

        //!2) Cache miss -> delegate the work to the factory method to get the cached shit
        var newValue =
            await factory()
            ?? throw new InvalidOperationException($"Factory method for key '{key}' returned null");

        //!3) Cache the new value
        await SetAsync(key, newValue, expiration);

        return newValue;
    }

    public Task<bool> RemoveAsync(string key)
    {
        _memoryCache.Remove(key);
        return Task.FromResult(true);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        where T : class
    {
        var options = new MemoryCacheEntryOptions();
        if (expiration.HasValue)
            options.SetAbsoluteExpiration(expiration.Value);

        _memoryCache.Set(key, value, options);
        return Task.CompletedTask;
    }
}
