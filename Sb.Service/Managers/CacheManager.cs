using System;
using System.Runtime.Caching;
using Sb.Interfaces.Services;

namespace Sb.Services.Managers
{
    public class CacheManager : ICacheManager
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;

        public bool Contains(string key)
        {
            return Cache.Contains(key);
        }

        public object Get(string key)
        {
            return Cache.Get(key);
        }

        public void Add(string key, object value)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddYears(1)
            };

            Cache.Add(key, value, cacheItemPolicy);
        }
    }
}