using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Domain.Entities;

namespace GeospatialLocation.Domain.Repositories
{
    public interface ILocationRepository
    {
        Task<long> InsertBulkGeospatialLocations(ICollection<Location> locations);
        Task CreateClusterAsync(Cluster cluster);
    }
}