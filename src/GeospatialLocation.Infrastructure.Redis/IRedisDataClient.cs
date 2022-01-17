using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace GeospatialLocation.Infrastructure.Redis
{
    public interface IRedisDataClient : IDisposable
    {
        ITransaction CreateTransaction();
        Task StringSetAsync(string key, byte[] bytes);
        Task<byte[][]> SortAsync(string key, string get);
        Task SetAddAsync(string key, ICollection<long> collection);
        Task<byte[]> StringGetAsync(string key);
    }
}