using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeospatialLocation.Application.ViewModels;
using GeospatialLocation.Domain.Entities;
using StackExchange.Redis;

namespace GeospatialLocation.Infrastructure.Redis
{
    public class RedisClient : IRedisClient
    {
        private readonly IRedisDataClient _dataClient;

        public RedisClient(IRedisDataClient dataClient)
        {
            _dataClient = dataClient;
        }

        public Task<long> AddGeoPoints(string key, ICollection<Location> locations)
        {
            return _dataClient.AddGeoPoints(key, locations.Select(CreateGeoEntry).ToArray());
        }

        public async Task<IEnumerable<LocationResultView>> GetLocations(string key, double lat,
            double lon, int maxDistance, int maxResults)
        {
            var results = await _dataClient.GetNearbyGeoPoints(key, lat, lon, maxDistance, maxResults);

            return results.Select(CreateLocationView).Take(maxResults);
        }

        private static GeoEntry CreateGeoEntry(Location location)
        {
            return new GeoEntry(
                location.Longitude, location.Latitude, location.Address
            );
        }

        private static LocationResultView CreateLocationView(GeoRadiusResult result)
        {
            var position = result.Position.GetValueOrDefault();
            return new LocationResultView(result.Member, result.Distance, position.Latitude, position.Longitude);
        }
    }
}