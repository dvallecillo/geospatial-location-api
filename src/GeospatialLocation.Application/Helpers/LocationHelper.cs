using System;
using System.Collections.Generic;
using System.Linq;
using GeospatialLocation.Application.Constants;
using GeospatialLocation.Domain.Entities;
using GeospatialLocation.Domain.Models;

namespace GeospatialLocation.Application.Helpers
{
    public static class LocationHelper
    {
        /// <summary>
        ///     Creates a new geographical point that is <paramref name="offsetLat" />, <paramref name="offsetLon" /> meters from
        ///     provided point location.
        /// </summary>
        public static Point Add(Point point, double offsetLat, double offsetLon)
        {
            var latitude = point.Latitude + offsetLat / 111111d;
            var longitude = point.Longitude + offsetLon / (111111d * Math.Cos(latitude));

            return new Point
            {
                Latitude = latitude,
                Longitude = longitude
            };
        }

        /// <summary>
        ///     Calculates the distance between 2 geographical points, in meters.
        /// </summary>
        public static double CalculateDistance(Point point1, Point point2)
        {
            var rlat1 = Math.PI * point1.Latitude / 180;
            var rlat2 = Math.PI * point2.Latitude / 180;
            var theta = point1.Longitude - point2.Longitude;
            var rtheta = Math.PI * theta / 180;
            var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return dist * 1609.344;
        }

        public static bool Overlaps(Point cluster, Boundary currentCluster)
        {
            return cluster.Latitude <= currentCluster.MaxLat &&
                   cluster.Latitude >= currentCluster.MinLat &&
                   cluster.Longitude <= currentCluster.MaxLon &&
                   cluster.Longitude >= currentCluster.MinLon;
        }

        public static Point CalculateCentroid(ICollection<Location> locations)
        {
            var locationAmount = locations.Count;
            var latitudeSum = locations.Sum(l => l.Latitude);
            var longitudeSum = locations.Sum(l => l.Longitude);

            return new Point
            {
                Latitude = latitudeSum / locationAmount,
                Longitude = longitudeSum / locationAmount
            };
        }

        public static void CombineCluster(Cluster cluster, Cluster match)
        {
            var location = cluster.Locations.First();
            match.Locations.Add(location);
            //TODO: I think it is not necessary
            //match.Center = CalculateCentroid(match.Locations);
        }

        public static bool IsValid(Location location)
        {
            return location.Latitude >= LocationConstants.MinLat && location.Longitude >= LocationConstants.MinLon &&
                   location.Latitude <= LocationConstants.MaxLat && location.Longitude <= LocationConstants.MaxLon;
        }
    }
}