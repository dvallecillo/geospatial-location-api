using System.Threading.Tasks;
using StackExchange.Redis;

namespace GeospatialLocation.Infrastructure.Redis
{
    public interface IRedisDataClient
    {
        Task<long> AddGeoPoints(string key, GeoEntry[] locations);
        Task<GeoRadiusResult[]> GetNearbyGeoPoints(string key, double lat, double lon, int maxDistance, int maxResults);
    }
}