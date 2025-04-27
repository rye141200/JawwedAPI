using System;
using System.Text.Json;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace JawwedAPI.Infrastructure.Repositories;

public class RedisCacheService(IDistributedCache distributedCache) : ICacheService
{
    private readonly JsonSerializerOptions jsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    public async Task<T?> GetAsync<T>(string key)
        where T : class
    {
        var cachedBytes = await distributedCache.GetAsync(key);

        if (cachedBytes == null)
            return null;

        var cachedJson = System.Text.Encoding.UTF8.GetString(cachedBytes);
        return JsonSerializer.Deserialize<T>(cachedJson, jsonOptions);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        where T : class
    {
        var json = JsonSerializer.Serialize(value);
        var bytes = System.Text.Encoding.UTF8.GetBytes(json);

        var options = new DistributedCacheEntryOptions();
        if (expiration.HasValue)
            options.SetAbsoluteExpiration(expiration.Value);

        await distributedCache.SetAsync(key, bytes, options);
    }

    public async Task<bool> RemoveAsync(string key)
    {
        await distributedCache.RemoveAsync(key);
        return true;
    }

    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? expiration = null
    )
        where T : class
    {
        // Try to get from cache first
        var cachedValue = await GetAsync<T>(key);
        if (cachedValue != null)
            return cachedValue;

        // Cache miss - get from factory
        var value = await factory();
        if (value != null)
            await SetAsync(key, value, expiration);

        return value!;
    }
}
