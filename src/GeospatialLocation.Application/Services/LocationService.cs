using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GeospatialLocation.Domain.Entities;

namespace GeospatialLocation.Application.Services
{
    public class LocationService : ILocationService
    {
        private const double ClusterRange = 0.01; // 1111 meters
        private const double MinLat = -90;
        private const double MaxLat = 90;
        private const double MinLon = -180;
        private const double MaxLon = 180;

        private readonly Dictionary<Guid, Cluster> _clusters = new();

        public void BulkLoadKdTree(ICollection<Location> locations)
        {
            if (locations == null) throw new ArgumentNullException();
                
            var initialClusters = locations.Where(IsValid).Select(CreateInitialCluster);

            foreach (var cluster in initialClusters)
            {
                if (InCluster(cluster, out match))
                {
                    CombineCluster;
                }
                else
                {
                    _clusters.Add(cluster.Id, cluster);
                }
            }
                
        }

        private bool InCluster(Cluster cluster, out Cluster match)
        {
            foreach (var currentCluster in _clusters)
            {
                cluster
            }

            return false;
        }


        private static Cluster CreateInitialCluster(Location location)
        {
            return new Cluster
            {
                Id = Guid.NewGuid(),
                Locations = new List<Location>()
                {
                    location
                },
                Center = new Point
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                },
                MinLat = location.Latitude - ClusterRange,
                MaxLat = location.Latitude + ClusterRange,
                MinLon = location.Longitude - ClusterRange,
                MaxLon = location.Longitude + ClusterRange,
            };
        }

        private static bool IsValid(Location location)
        {
            return location.Latitude >= MinLat && location.Longitude >= MinLon &&
                   location.Latitude <= MaxLat && location.Longitude <= MaxLon;
        }
    }

    class Point
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    class Cluster
    {
        public Guid Id { get; set; }
        public ICollection<Location> Locations { get; set; }
        public Point Center { get; set; }

        //Draw the square of the cluster
        public double MinLat { get; set; }
        public double MaxLat { get; set; }
        
        public double MinLon { get; set; }
        public double MaxLon { get; set; }

    }
}