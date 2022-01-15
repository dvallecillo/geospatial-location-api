using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Domain.Entities;

namespace GeospatialLocation.Application.Queries
{
    public interface ILocationQueries
    {
        Task<IEnumerable<Cluster>> GetClustersAsync(double requestLat, double requestLon,
            int requestMaxDistance, int requestMaxResults);
    }
}