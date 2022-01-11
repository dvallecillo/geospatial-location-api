using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Application.ViewModels;

namespace GeospatialLocation.Application.Queries
{
    public interface ILocationQueries
    {
        Task<IEnumerable<LocationResultView>>
            GetLocationsAsync(double lat, double lon, int maxDistance, int maxResults);
    }
}