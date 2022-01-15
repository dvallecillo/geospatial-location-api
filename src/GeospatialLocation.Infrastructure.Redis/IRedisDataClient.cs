using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace GeospatialLocation.Infrastructure.Redis
{
    public interface IRedisDataClient : IDisposable
    {
        ITransaction CreateTransaction();
        Task StringSetAsync(string key, byte[] bytes);
        Task SetAddAsync(string key, Guid id);
        Task<byte[][]> SortAsync(string key, string get);
    }
}