using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace GeospatialLocation.Infrastructure.Redis
{
    public class RedisDataClient : IRedisDataClient
    {
        private static readonly Lazy<ConnectionMultiplexer> Connection
            = new(RedisConnectionFactory.CreateConnection);

        private readonly IDatabase _database;

        public RedisDataClient()
        {
            _database = Redis.GetDatabase();
        }

        public ConnectionMultiplexer Redis => Connection.Value;

        public Task<long> AddGeoPoints(string key, GeoEntry[] locations)
        {
            return _database.GeoAddAsync(key, locations);
        }

        public Task<GeoRadiusResult[]> GetNearbyGeoPoints(string key, double lat, double lon, int maxDistance,
            int maxResults)
        {
            return _database.GeoRadiusAsync(key, lon, lat, maxDistance, GeoUnit.Meters, maxResults, Order.Ascending);
        }
    }
}