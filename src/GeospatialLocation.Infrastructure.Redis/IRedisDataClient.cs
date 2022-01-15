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
        Task<long> AddGeoPoints(string key, GeoEntry[] locations);
        Task<GeoRadiusResult[]> GetNearbyGeoPoints(string key, double lat, double lon, int maxDistance, int maxResults);
        Task<byte[][]> SortAsync(string key, string get);
    }
}