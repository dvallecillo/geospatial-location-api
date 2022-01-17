using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeospatialLocation.Domain.Entities;
using GeospatialLocation.Domain.Models;
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

        public async Task CreateClustersAsync(ICollection<Cluster> clusters)
        {
            await using var transaction =
                await _unitOfWork.BeginTransactionAsync();

            Client.AddToSet(KeyHelper.ClusterCollectionKey, clusters.Select(c => c.Id).ToList());

            foreach (var cluster in clusters)
            {
                var key = string.Format(KeyHelper.ClusterDetailKey, cluster.Id);
                await Client.SetAsync(key, cluster);

                var centerKey = string.Format(KeyHelper.ClusterCenterKey, cluster.Id);
                await Client.SetAsync(centerKey, new KeyValuePair<long, Point>(cluster.Id, cluster.Center));
            }

            await _unitOfWork.CommitTransactionAsync(transaction);
        }
    }
}