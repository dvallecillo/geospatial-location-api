using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Domain.Entities;
using GeospatialLocation.Domain.Repositories;

namespace GeospatialLocation.Infrastructure.Redis.Repositories
{
    public class LocationRepository : RedisRepository, ILocationRepository
    {
        private const string CollectionKey = "locations";

        public LocationRepository(IRedisClient client) : base(client)
        {
        }

        public Task<long> InsertBulkLocations(ICollection<Location> locations)
        {
            return Client.AddGeoPoints(CollectionKey, locations);
        }
    }
}