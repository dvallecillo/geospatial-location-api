using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Domain.Entities;
using GeospatialLocation.Domain.Models;

namespace GeospatialLocation.Application.Queries
{
    public interface ILocationQueries
    {
        Task<Cluster> GetClusterAsync(long id);
        Task<IEnumerable<KeyValuePair<long, Point>>> GetClusterCentersAsync();
    }
}