using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Application.ViewModels;
using GeospatialLocation.Domain.Entities;

namespace GeospatialLocation.Infrastructure.Redis
{
    public interface IRedisClient
    {
        Task<long> AddGeoPoints(string key, ICollection<Location> location);

        Task<IEnumerable<LocationResultView>> GetLocations(string key,
            double lat, double lon, int maxDistance, int maxResults);
    }
}