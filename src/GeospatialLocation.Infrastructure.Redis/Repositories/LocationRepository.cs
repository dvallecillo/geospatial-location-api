using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Domain.Entities;
using GeospatialLocation.Domain.Repositories;
using GeospatialLocation.Domain.SeedWork;
using GeospatialLocation.Infrastructure.Redis.Helpers;

namespace GeospatialLocation.Infrastructure.Redis.Repositories
{
    public class LocationRepository : RedisRepository, ILocationRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationRepository(IRedisClient client, IUnitOfWork unitOfWork) : base(client)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<long> InsertBulkGeospatialLocations(ICollection<Location> locations)
        {
            return Client.AddGeoPoints(KeyHelper.GeospatialIndexCollectionKey, locations);
        }

        public async Task CreateClusterAsync(Cluster cluster)
        {
            await using var transaction =
                await _unitOfWork.BeginTransactionAsync();

            //TODO: Add cluster distances to be more efficient
            Client.AddToSet(KeyHelper.ClusterCollectionKey, cluster.Id);

            var key = string.Format(KeyHelper.ClusterDetailKey, cluster.Id);
            await Client.SetAsync(key, cluster);

            await _unitOfWork.CommitTransactionAsync(transaction);
        }
    }
}