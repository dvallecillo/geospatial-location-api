using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Application.Queries;
using GeospatialLocation.Domain.Entities;
using GeospatialLocation.Infrastructure.Redis.Helpers;

namespace GeospatialLocation.Infrastructure.Redis.Queries
{
    public class LocationQueries : RedisRepository, ILocationQueries
    {
        public LocationQueries(IRedisClient client) : base(client)
        {
        }

        public async Task<IEnumerable<Cluster>> GetClustersAsync()
        {
            return await FindClusters(KeyHelper.ClusterCollectionKey);
        }

        private Task<IEnumerable<Cluster>> FindClusters(string setName)
        {
            var get = string.Format(KeyHelper.ClusterDetailKey, "*");
            return Client.GetSortedAsync<Cluster>(setName, get);
        }
    }
}