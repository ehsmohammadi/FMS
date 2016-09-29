using System;
using System.Collections.Generic;
using System.Web;
using MITD.Domain.Model;

namespace MITD.Fuel.Service.Host.Infrastructure
{
    public class HostIdGenerator : IIDGenerator
    {
        private const string _cacheKey = "IDCache";

        public List<int> Generate<T>(int stackSize) where T : class
        {
            var cache =this.GetCache();
            if (cache == null)
                cache = this.CreateCache(stackSize);

            if (stackSize == 1)
                return new List<int> { this.UseID(cache) };

            var result = new List<int>();
            for (int i = 0; i < stackSize; i++)
            {
                result.Add(this.UseID(cache));
            }
            return result;
        }

        public int Generate<T>() where T : class
        {
            var result = this.Generate<T>(1);
            return result[0];
        }

        private int UseID(IDCache cache)
        {
            cache.LastValue = cache.LastValue+1;
            return cache.LastValue;
        }

        private IDCache GetCache( )
        {
            var cache = HttpContext.Current.Application[_cacheKey] as IDCache;
            return cache;
        }

        private IDCache CreateCache(int stackSize)
        {

            var start =Convert.ToInt32( string.Concat(DateTime.Now.DayOfYear, DateTime.Now.ToString("HHmmss"), 0));
           
            var cache = new IDCache()
                            {
                                LastValue =start 
                            };
            HttpContext.Current.Application[_cacheKey] = cache;

            return cache;
        }

        private class IDCache
        {
            public int LastValue { get; set; }

        }
    }
}