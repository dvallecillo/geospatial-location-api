using System;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace GeospatialLocation.Infrastructure.Redis
{
    public class RedisDataClient : IRedisDataClient
    {
        private static readonly Lazy<ConnectionMultiplexer> Connection
            = new(RedisConnectionFactory.CreateConnection);

        private readonly IDatabase _database;
        private ITransaction? _transaction;

        public RedisDataClient()
        {
            _database = Redis.GetDatabase();
        }

        public ConnectionMultiplexer Redis => Connection.Value;

        public ITransaction CreateTransaction()
        {
            _transaction = _database.CreateTransaction();
            return _transaction;
        }

        public Task StringSetAsync(string key, byte[] bytes)
        {
            return Task.FromResult(
                _transaction != null
                    ? _transaction.StringSetAsync(key, bytes)
                    : _database.StringSetAsync(key, bytes));
        }

        public Task SetAddAsync(string key, Guid id)
        {
            return _transaction != null
                ? _transaction.SetAddAsync(key, id.ToString())
                : _database.SetAddAsync(key, id.ToString());
        }

        public async Task<byte[][]> SortAsync(string key, string get)
        {
            var members = await _database.SortAsync(key, 0, -1, Order.Ascending, SortType.Alphabetic, default,
                new RedisValue[] { get });

            return members.Select(x => (byte[])x).ToArray();
        }

        //GEOSPATIAL INDEX PART

        public Task<long> AddGeoPoints(string key, GeoEntry[] locations)
        {
            return _database.GeoAddAsync(key, locations);
        }

        public Task<GeoRadiusResult[]> GetNearbyGeoPoints(string key, double lat, double lon, int maxDistance,
            int maxResults)
        {
            return _database.GeoRadiusAsync(key, lon, lat, maxDistance, GeoUnit.Meters, maxResults, Order.Ascending);
        }

        public void Dispose()
        {
            _transaction = null;
            GC.SuppressFinalize(this);
        }

        ///
    }
}