using System;
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
        private readonly ISerializer _serializer;

        public RedisClient(IRedisDataClient dataClient, ISerializer serializer)
        {
            _dataClient = dataClient;
            _serializer = serializer;
        }

        public Task AddToSetAsync(string key, Guid id)
        {
            return _dataClient.SetAddAsync(key, id);
        }

        public Task SetAsync<T>(string key, T data)
        {
            var bytes = _serializer.Serialize(data);
            return _dataClient.StringSetAsync(key, bytes);
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