using System.Threading.Tasks;
using GeospatialLocation.Domain.Entities;

namespace GeospatialLocation.Domain.Repositories
{
    public interface ILocationRepository
    {
        Task CreateClusterAsync(Cluster cluster);
    }
}