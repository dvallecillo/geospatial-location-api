using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Application.Queries;
using GeospatialLocation.Application.ViewModels;

namespace GeospatialLocation.Infrastructure.Redis.Queries
{
    public class LocationQueries : RedisRepository, ILocationQueries
    {
        private const string CollectionKey = "locations";

        public LocationQueries(IRedisClient client) : base(client)
        {
        }

        public async Task<IEnumerable<LocationResultView>> GetLocationsAsync(double lat, double lon, int maxDistance,
            int maxResults)
        {
            return await Client.GetLocations(CollectionKey, lat, lon, maxDistance, maxResults);
        }
    }
}