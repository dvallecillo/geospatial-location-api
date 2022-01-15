using System.Collections.Generic;
using GeospatialLocation.Domain.Entities;

namespace GeospatialLocation.Application.Comparers
{
    public class LocationEqualityComparer : IEqualityComparer<Location>
    {
        public bool Equals(Location x, Location y)
        {
            // Two items are equal if their keys are equal.
            return x.Address == y.Address;
        }

        public int GetHashCode(Location obj)
        {
            return obj.Address.GetHashCode();
        }
    }
}