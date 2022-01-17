using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Application.Queries;
using GeospatialLocation.Domain.Entities;
using GeospatialLocation.Domain.Models;
using GeospatialLocation.Infrastructure.Redis.Helpers;

namespace GeospatialLocation.Infrastructure.Redis.Queries
{
    public class LocationQueries : RedisRepository, ILocationQueries
    {
        public LocationQueries(IRedisClient client) : base(client)
        {
        }

        public async Task<Cluster> GetClusterAsync(long id)
        {
            return await Client.GetAsync<Cluster>(string.Format(KeyHelper.ClusterDetailKey, id));
        }

        public async Task<IEnumerable<KeyValuePair<long, Point>>> GetClusterCentersAsync()
        {
            return await FindClusterCenters(KeyHelper.ClusterCollectionKey);
        }

        private Task<IEnumerable<KeyValuePair<long, Point>>> FindClusterCenters(string setName)
        {
            var get = string.Format(KeyHelper.ClusterCenterKey, "*");
            return Client.GetSortedAsync<KeyValuePair<long, Point>>(setName, get);
        }
    }
}