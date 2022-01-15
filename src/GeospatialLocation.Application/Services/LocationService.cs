using System;
using System.Collections.Generic;
using GeospatialLocation.Domain.Entities;

namespace GeospatialLocation.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly Dictionary<Guid, Cluster> _clusters = new();

        //public void BulkLoadLocations(ICollection<Location> locations)
        //{
        //    if (locations == null)
        //    {
        //        throw new ArgumentNullException();
        //    }

        //    var initialClusters = locations.Distinct(new LocationEqualityComparer()).Where(IsValid)
        //        .Select(CreateInitialCluster).ToList();

        //    if (!initialClusters.Any())
        //    {
        //        return;
        //    }

        //    for (var i = 1; i < initialClusters.Count; i++)
        //    {
        //        if (InCluster(initialClusters[i], out var match))
        //        {
        //            CombineCluster(initialClusters[i], match);
        //        }
        //        else
        //        {
        //            _clusters.Add(initialClusters[i].Id, initialClusters[i]);
        //        }
        //    }

        //    //var ok = initialClusters.Count == _clusters.Sum(c => c.Value.Locations.Count);

        //    // QUERY LOCATIONS
        //    var queryLocation = new Point
        //    {
        //        Latitude = 52.3803504,
        //        Longitude = 4.6226071
        //    };
        //    var queryMaxDistance = 200; // CLUSTER DEMASIADO GRANDE?
        //    var queryMaxResults = 50;


        //    var clusterDistances = _clusters
        //        .ToDictionary(c => c.Key,
        //            c =>
        //                LocationHelper.CalculateDistance(queryLocation, c.Value.Center))
        //        .Where(c => c.Value <= queryMaxDistance + ClusterRange).OrderBy(c => c.Value);

        //    var results = new Dictionary<string, double>();
        //    foreach (var clusterDistance in clusterDistances)
        //    {
        //        var clusterLocations = _clusters[clusterDistance.Key].Locations
        //            .ToDictionary(
        //                l => l.Address,
        //                l => LocationHelper.CalculateDistance(
        //                    new Point { Latitude = l.Latitude, Longitude = l.Longitude }, queryLocation)
        //            ).OrderBy(l => l.Value);
        //        Console.WriteLine(clusterLocations);
        //    }
        //}

        //private static void CombineCluster(Cluster cluster, Cluster match)
        //{
        //    var location = cluster.Locations.First();
        //    match.Locations.Add(location);
        //    match.Center = CalculateCentroid(match.Locations);
        //    //match.Boundary = new Boundary(point, ClusterRange);
        //}

        //private static Point CalculateCentroid(ICollection<Location> locations)
        //{
        //    var locationAmount = locations.Count;
        //    var latitudeSum = locations.Sum(l => l.Latitude);
        //    var longitudeSum = locations.Sum(l => l.Longitude);

        //    return new Point
        //    {
        //        Latitude = latitudeSum / locationAmount,
        //        Longitude = longitudeSum / locationAmount
        //    };
        //}

        //private bool InCluster(Cluster cluster, out Cluster match)
        //{
        //    match = null;
        //    foreach (var (_, value) in _clusters)
        //    {
        //        if (Overlaps(cluster.Center, value.Boundary))
        //        {
        //            match = value;
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //private static bool Overlaps(Point cluster, Boundary currentCluster)
        //{
        //    return cluster.Latitude <= currentCluster.MaxLat &&
        //           cluster.Latitude >= currentCluster.MinLat &&
        //           cluster.Longitude <= currentCluster.MaxLon &&
        //           cluster.Longitude >= currentCluster.MinLon;
        //}

        //private static Cluster CreateInitialCluster(Location location)
        //{
        //    var center = new Point
        //    {
        //        Latitude = location.Latitude,
        //        Longitude = location.Longitude
        //    };
        //    return new Cluster
        //    {
        //        Id = Guid.NewGuid(),
        //        Locations = new List<Location>
        //        {
        //            location
        //        },
        //        Center = center,
        //        Boundary = new Boundary(center, ClusterRange)
        //    };
        //}

        //private static bool IsValid(Location location)
        //{
        //    return location.Latitude >= MinLat && location.Longitude >= MinLon &&
        //           location.Latitude <= MaxLat && location.Longitude <= MaxLon;
        //}

        //private class LocationEqualityComparer : IEqualityComparer<Location>
        //{
        //    public bool Equals(Location x, Location y)
        //    {
        //        // Two items are equal if their keys are equal.
        //        return x.Address == y.Address;
        //    }

        //    public int GetHashCode(Location obj)
        //    {
        //        return obj.Address.GetHashCode();
        //    }
        //}

        //internal class SquareLimits
        //{
        //    public SquareLimits(Location location, int distance)
        //    {
        //        var latitudesUpdated = new List<double>
        //        {
        //            location.Add(distance, 0).Latitude,
        //            location.Add(-1 * distance, 0).Latitude
        //        };
        //        var longitudesUpdated = new List<double>
        //        {
        //            location.Add(0, distance).Longitude,
        //            location.Add(0, -1 * distance).Longitude
        //        };

        //        MaxLatitude = latitudesUpdated.Max();
        //        MinLatitude = latitudesUpdated.Min();

        //        MaxLongitude = longitudesUpdated.Max();
        //        MinLongitude = longitudesUpdated.Min();

        //        Boundary = new Boundary
        //        {
        //            MinLat = MinLatitude,
        //            MaxLat = MaxLatitude,
        //            MinLon = MinLongitude,
        //            MaxLon = MaxLongitude
        //        };
        //    }

        //    public Boundary Boundary { get; set; }
        //    public double MaxLatitude { get; set; }
        //    public double MaxLongitude { get; set; }
        //    public double MinLatitude { get; set; }
        //    public double MinLongitude { get; set; }
        //}
        public void BulkLoadLocations(ICollection<Location> locations)
        {
            throw new NotImplementedException();
        }
    }

    //public class Point
    //{
    //    public double Latitude { get; set; }
    //    public double Longitude { get; set; }
    //}

    //public class Boundary
    //{
    //    public Boundary(Point point, int distance)
    //    {
    //        var latitudesUpdated = new List<double>
    //        {
    //            LocationHelper.Add(point, distance, 0).Latitude,
    //            LocationHelper.Add(point, -1 * distance, 0).Latitude
    //        };
    //        var longitudesUpdated = new List<double>
    //        {
    //            LocationHelper.Add(point, 0, distance).Longitude,
    //            LocationHelper.Add(point, 0, -1 * distance).Longitude
    //        };

    //        MinLat = latitudesUpdated.Min();
    //        MaxLat = latitudesUpdated.Max();
    //        MinLon = longitudesUpdated.Min();
    //        MaxLon = longitudesUpdated.Max();
    //    }

    //    public double MinLat { get; set; }
    //    public double MaxLat { get; set; }
    //    public double MinLon { get; set; }
    //    public double MaxLon { get; set; }
    //}

    //internal class Cluster
    //{
    //    public Guid Id { get; set; }
    //    public ICollection<Location> Locations { get; set; }
    //    public Point Center { get; set; }
    //    public Boundary Boundary { get; set; }
    //}
}