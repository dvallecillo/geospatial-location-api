using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Application.ViewModels;
using GeospatialLocation.Domain.Entities;

namespace GeospatialLocation.Application.Queries
{
    public interface ILocationQueries
    {
        Task<IEnumerable<LocationResultView>> GetRedisIndexLocationsAsync(double lat, double lon, int maxDistance,
            int maxResults);

        Task<IEnumerable<Cluster>> GetClustersAsync(double requestLat, double requestLon,
            int requestMaxDistance, int requestMaxResults);
    }
}