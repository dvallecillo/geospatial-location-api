using System;
using System.Collections.Generic;
using GeospatialLocation.Domain.Models;
using GeospatialLocation.Domain.SeedWork;

namespace GeospatialLocation.Domain.Entities
{
    public class Cluster : IResource
    {
        public ICollection<Location> Locations { get; set; }
        public Point Center { get; set; }
        public Boundary Boundary { get; set; }
        public Guid Id { get; set; }
    }
}