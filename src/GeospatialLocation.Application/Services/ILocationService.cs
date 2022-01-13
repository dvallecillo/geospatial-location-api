using System.Collections.Generic;
using GeospatialLocation.Domain.Entities;

namespace GeospatialLocation.Application.Services
{
    public interface ILocationService
    {
        void BulkLoadKdTree(ICollection<Location> locations);
    }
}