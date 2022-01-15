using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Application.Queries;
using GeospatialLocation.Application.ViewModels;
using GeospatialLocation.Infrastructure.Redis.Helpers;

namespace GeospatialLocation.Infrastructure.Redis.Queries
{
    public class LocationQueries : RedisRepository, ILocationQueries
    {
        public LocationQueries(IRedisClient client) : base(client)
        {
        }

        public async Task<IEnumerable<LocationResultView>> GetRedisIndexLocationsAsync(double lat, double lon,
            int maxDistance,
            int maxResults)
        {
            return await Client.GetLocations(KeyHelper.GeospatialIndexCollectionKey, lat, lon, maxDistance, maxResults);
        }
    }
}