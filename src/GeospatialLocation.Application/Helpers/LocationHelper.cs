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
        private const double MultiplierToMeter = 20014123.8528 / Math.PI; //20014123.8528 = 180 * 60 * 1.1515 * 1609.344

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

            return dist * MultiplierToMeter;
        }

        /// <summary>
        ///     Calculates if a lat/lon point is inside a boundary.
        /// </summary>
        public static bool InsideBoundary(Point point, Boundary boundary)
        {
            return point.Latitude <= boundary.MaxLat &&
                   point.Latitude >= boundary.MinLat &&
                   point.Longitude <= boundary.MaxLon &&
                   point.Longitude >= boundary.MinLon;
        }

        public static bool IsValid(Point point)
        {
            return point.Latitude >= LocationConstants.MinLat && point.Longitude >= LocationConstants.MinLon &&
                   point.Latitude <= LocationConstants.MaxLat && point.Longitude <= LocationConstants.MaxLon;
        }

        public static Boundary CreateBoundary(Point point)
        {
            var latitudesOffset = new List<double>
            {
                Add(point, LocationConstants.ClusterRange, 0).Latitude,
                Add(point, -1 * LocationConstants.ClusterRange, 0).Latitude
            };
            var longitudesOffset = new List<double>
            {
                Add(point, 0, LocationConstants.ClusterRange).Longitude,
                Add(point, 0, -1 * LocationConstants.ClusterRange).Longitude
            };

            return new Boundary
            {
                MinLat = latitudesOffset.Min(),
                MaxLat = latitudesOffset.Max(),
                MinLon = longitudesOffset.Min(),
                MaxLon = longitudesOffset.Max()
            };
        }

        public static Cluster CreateCluster(Location location)
        {
            var center = new Point
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
            return new Cluster
            {
                Id = Guid.NewGuid(),
                Locations = new List<Location>
                {
                    location
                },
                Center = center,
                Boundary = CreateBoundary(center)
            };
        }
    }
}