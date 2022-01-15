using System.Collections.Generic;
using GeospatialLocation.Domain.Entities;

namespace GeospatialLocation.Application.Services
{
    public interface ILocationService
    {
        void BulkLoadLocations(ICollection<Location> locations);
    }
}