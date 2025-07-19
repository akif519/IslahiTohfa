using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                return await Task.FromResult(_cache.Get<T>(key));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cache key: {Key}", key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var options = new MemoryCacheEntryOptions();

                if (expiration.HasValue)
                {
                    options.SetAbsoluteExpiration(expiration.Value);
                }
                else
                {
                    options.SetSlidingExpiration(TimeSpan.FromMinutes(30));
                }

                _cache.Set(key, value, options);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cache key: {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                _cache.Remove(key);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cache key: {Key}", key);
            }
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            try
            {
                // Note: IMemoryCache doesn't support pattern removal natively
                // This would require a more sophisticated caching solution like Redis
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cache by pattern: {Pattern}", pattern);
            }
        }
    }
}
