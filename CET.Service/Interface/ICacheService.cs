using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET.Service.Interface
{
    public interface ICacheService
    {
        Task<T?> GetCacheAsync<T>(string cacheKey);
        Task SetCacheAsync<T>(string cacheKey, T value, TimeSpan? ttl = null);
        Task RemoveCacheAsync<T>(string cacheKey);
        
    }
}
