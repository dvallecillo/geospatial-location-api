using System;
using System.Threading.Tasks;

namespace GeospatialLocation.Infrastructure.Redis
{
    public interface IRedisDataClient : IDisposable
    {
        Task<byte[]> StringGetAsync(string key);
        Task StringSetAsync(string key, byte[] bytes);
        Task StringSetAsync(string key, string value);
        Task<long> IncrementAsync(string key);
        Task SetAddAsync(string key, long id);
        Task SetRemoveAsync(string key, long id);
        Task<bool> ExistsAsync(string key);
        Task<byte[]?> HashGetAsync(string key, string field);
        Task HashSetAsync(string key, string hash, byte[] value);
        Task<long[]> GetSetAsIntAsync(string key);

        Task HashDeleteAsync(string key, string field);
    }
}