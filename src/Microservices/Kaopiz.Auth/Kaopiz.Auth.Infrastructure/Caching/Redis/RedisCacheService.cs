using Kaopiz.Auth.Application;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Kaopiz.Auth.Infrastructure
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetCacheAsync<T>(string cacheKey)
        {
            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (string.IsNullOrEmpty(cachedData))
                return default;

            return JsonConvert.DeserializeObject<T>(cachedData);
        }

        public async Task RemoveCacheAsync<T>(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task SetCacheAsync<T>(string cacheKey, T value, TimeSpan? ttl = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl ?? TimeSpan.FromMinutes(30)
            };

            var serializedData = JsonConvert.SerializeObject(value);
            await _cache.SetStringAsync(cacheKey, serializedData, options);
        }
    }
}
