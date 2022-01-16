using System;
using GeospatialLocation.Application.Constants;
using GeospatialLocation.Application.Helpers;
using GeospatialLocation.Domain.Models;
using GeospatialLocation.Tests.Utils;
using Xunit;

namespace GeospatialLocation.Tests.Application.Helpers
{
    public class LocationHelperTests
    {
        [Fact]
        public void
            InsideBoundary_PointIsInside_ReturnsTrue()
        {
            //Arrange
            var boundary = GetValidBoundary();
            var randomPoint = new Point
            {
                Latitude = GetRandomDouble(boundary.MinLat, boundary.MaxLat),
                Longitude = GetRandomDouble(boundary.MinLon, boundary.MaxLon)
            };

            //Act
            var extensionResult = LocationHelper.InsideBoundary(randomPoint, boundary);

            // Assert
            Assert.True(extensionResult);
        }

        [Fact]
        public void
            InsideBoundary_PointIsOutside_ReturnsFalse()
        {
            //Arrange
            var boundary = GetValidBoundary();
            var randomPoint = new Point
            {
                Latitude = GetRandomDouble(boundary.MaxLat, LocationConstants.MaxLat + 1),
                Longitude = GetRandomDouble(boundary.MaxLon, LocationConstants.MaxLon + 1)
            };

            //Act
            var extensionResult = LocationHelper.InsideBoundary(randomPoint, boundary);

            // Assert
            Assert.False(extensionResult);
        }

        [Fact]
        public void
            IsValid_IsValidLatLon_ReturnsTrue()
        {
            //Arrange
            var randomPoint = new Point
            {
                Latitude = GetRandomDouble(LocationConstants.MinLat, LocationConstants.MaxLat),
                Longitude = GetRandomDouble(LocationConstants.MinLon, LocationConstants.MaxLon)
            };

            //Act
            var extensionResult = LocationHelper.IsValid(randomPoint);

            // Assert
            Assert.True(extensionResult);
        }

        [Fact]
        public void
            IsValid_IsInvalidLatLon_ReturnsFalse()
        {
            //Arrange
            var randomPoint = new Point
            {
                Latitude = GetRandomDouble(LocationConstants.MaxLat, LocationConstants.MaxLat + 1),
                Longitude = GetRandomDouble(LocationConstants.MaxLon, LocationConstants.MaxLon + 1)
            };

            //Act
            var extensionResult = LocationHelper.IsValid(randomPoint);

            // Assert
            Assert.False(extensionResult);
        }

        [Theory]
        [AutoMoqData]
        public void
            CalculateDistance_SameResults_ReturnsSameResults(Point origin,
                Point target)
        {
            //Arrange
            //Act
            var extensionResult = LocationHelper.CalculateDistance(origin, target);
            var originalResult = CalculateDistance(origin, target);

            // Assert
            Assert.True(Math.Abs(extensionResult - originalResult) < 0.00001);
        }

        private Boundary GetValidBoundary()
        {
            var maxLat = GetRandomLat();
            var minLat = maxLat - 0.1;
            var maxLon = GetRandomLon();
            var minLon = maxLon - 0.1;
            return new Boundary
            {
                MinLat = minLat,
                MaxLat = maxLat,
                MinLon = minLon,
                MaxLon = maxLon
            };
        }

        private static double GetRandomLat()
        {
            return GetRandomDouble(LocationConstants.MinLat, LocationConstants.MaxLat);
        }

        private static double GetRandomLon()
        {
            return GetRandomDouble(LocationConstants.MinLon, LocationConstants.MaxLon);
        }

        private static double GetRandomDouble(double minimum, double maximum)
        {
            var random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        private static double CalculateDistance(Point origin, Point target)
        {
            var rlat1 = Math.PI * origin.Latitude / 180;
            var rlat2 = Math.PI * target.Latitude / 180;
            var theta = origin.Longitude - target.Longitude;
            var rtheta = Math.PI * theta / 180;
            var dist = Math.Sin(rlat1) * Math.Sin(rlat2) +
                       Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return dist * 1609.344;
        }
    }
}