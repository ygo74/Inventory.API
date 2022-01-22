using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Infrastructure.Services
{
    public class CachingService
    {
        private readonly IMemoryCache _cache;

        public CachingService(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));

        }

        public async Task<T> UseCacheAsync<T>(Func<Task<T>> internalMethod, string cacheKey)
        {

            //Try to obtain the variables from the cache
            T outputValue;
            if (_cache.TryGetValue(cacheKey, out outputValue))
            {
                return outputValue;
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions();
            outputValue = await internalMethod.Invoke();
            _cache.Set(cacheKey, outputValue, cacheEntryOptions);

            return outputValue;

        }

    }
}
