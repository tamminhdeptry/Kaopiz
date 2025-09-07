namespace Kaopiz.Auth.Application
{
    public interface ICacheService
    {
        Task<T?> GetCacheAsync<T>(string cacheKey);
        Task SetCacheAsync<T>(string cacheKey, T value, TimeSpan? ttl = null);
        Task RemoveCacheAsync<T>(string cacheKey);
        
    }
}
