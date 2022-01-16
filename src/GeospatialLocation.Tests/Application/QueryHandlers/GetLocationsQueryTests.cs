using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using GeospatialLocation.Application.Constants;
using GeospatialLocation.Application.Exceptions;
using GeospatialLocation.Application.Exceptions.Errors;
using GeospatialLocation.Application.Helpers;
using GeospatialLocation.Application.Queries;
using GeospatialLocation.Application.ViewModels;
using GeospatialLocation.Domain.Entities;
using GeospatialLocation.Domain.Models;
using GeospatialLocation.Tests.Utils;
using Moq;
using Xunit;

namespace GeospatialLocation.Tests.Application.QueryHandlers
{
    public class QueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task
            Handle_NegativeDistance_ThrowsNegativeDistanceException(
                double latitude,
                double longitude,
                [Frozen] Mock<ILocationQueries> locationQueries,
                GetLocationsQueryHandler sut)
        {
            //Arrange
            var randomize = new Random();
            var distance = randomize.Next(int.MinValue, 0);
            var maxResults = randomize.Next(int.MaxValue);
            var request = new GetLocationsQuery(latitude, longitude, distance, maxResults);

            //Act
            Func<Task> act = () => sut.Handle(request, default);
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage(new BusinessException(RequestErrors.NegativeMaxDistance).MessageTemplate);

            //Assert
            locationQueries.Verify(
                x => x.GetClustersAsync(), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task
            Handle_SmallerThanOneMaxResults_ThrowsInvalidMaxResultsException(
                double latitude,
                double longitude,
                [Frozen] Mock<ILocationQueries> locationQueries,
                GetLocationsQueryHandler sut)
        {
            //Arrange
            var randomize = new Random();
            var distance = randomize.Next(int.MaxValue);
            var maxResults = randomize.Next(int.MinValue, 1);
            ;
            var request = new GetLocationsQuery(latitude, longitude, distance, maxResults);

            //Act 
            Func<Task> act = () => sut.Handle(request, default);
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage(new BusinessException(RequestErrors.InvalidMaxResults).MessageTemplate);

            //Assert
            locationQueries.Verify(
                x => x.GetClustersAsync(), Times.Never);
        }


        [Theory]
        [AutoMoqData]
        public async Task
            Handle_InvalidRequestPoint_ThrowsRequestLatLonOutOfRangeException(
                double longitude,
                [Frozen] Mock<ILocationQueries> locationQueries,
                GetLocationsQueryHandler sut)
        {
            //Arrange
            const double latitude = LocationConstants.MaxLat + 1;
            var request = new GetLocationsQuery(latitude, longitude, new Random().Next(int.MaxValue),
                new Random().Next(int.MaxValue));

            //Act
            Func<Task> act = () => sut.Handle(request, default);
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage(new BusinessException(RequestErrors.RequestLatLonOutOfRange).MessageTemplate);

            //Assert
            locationQueries.Verify(
                x => x.GetClustersAsync(), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task
            Handle_CorrectRequestWithResult_ReturnLocations(
                [Frozen] Mock<ILocationQueries> locationQueries,
                GetLocationsQueryHandler sut)
        {
            //Arrange
            const double lat = 0.011;
            const double lon = 0.123;
            var requestPoint = new Point
            {
                Latitude = lat,
                Longitude = lon
            };
            var request = new GetLocationsQuery(lat, lon, 500, 5);

            var location1 = new Location
            {
                Address = "Address 1",
                Latitude = lat,
                Longitude = lon
            };

            var location2 = new Location
            {
                Address = "Address 2",
                Latitude = lat + 0.001,
                Longitude = lon + 0.001
            };

            var location3 = new Location
            {
                Address = "Address 3",
                Latitude = lat + 0.002,
                Longitude = lon + 0.002
            };

            var locations = new List<Location> { location1, location2, location3 };

            var cluster = new Cluster
            {
                Locations = locations,
                Center = location2.Point,
                Boundary = LocationHelper.CreateBoundary(location2.Point),
                Id = Guid.NewGuid()
            };

            var clusters = new List<Cluster> { cluster };

            var expectedResult = new List<LocationResultView>();
            expectedResult.AddRange(locations.Select(location => new LocationResultView(location.Address,
                LocationHelper.CalculateDistance(location.Point, requestPoint), location.Latitude,
                location.Longitude)));
            expectedResult = expectedResult.Where(r => r.Distance <= request.MaxDistance).OrderBy(r => r.Distance)
                .Take(request.MaxResults).ToList();

            locationQueries.Setup(
                    x => x.GetClustersAsync())
                .Returns(Task.FromResult((IEnumerable<Cluster>)clusters));

            //Act
            var result = (await sut.Handle(request, default)).ToList();

            ////Assert
            result.Should().BeEquivalentTo(expectedResult);
            result.Should().HaveCount(expectedResult.Count);
            locationQueries.Verify(
                x => x.GetClustersAsync(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task
            Handle_CorrectRequestWithMaxDistanceConstraint_ReturnLocationsOnRange(
                [Frozen] Mock<ILocationQueries> locationQueries,
                GetLocationsQueryHandler sut)
        {
            //Arrange
            const double lat = 0.011;
            const double lon = 0.123;
            var requestPoint = new Point
            {
                Latitude = lat,
                Longitude = lon
            };
            var request = new GetLocationsQuery(requestPoint.Latitude, requestPoint.Longitude, 300, 5);

            var location1 = new Location
            {
                Address = "Address 1",
                Latitude = requestPoint.Latitude,
                Longitude = lon
            };

            var location2 = new Location
            {
                Address = "Address 2",
                Latitude = requestPoint.Latitude + 0.001,
                Longitude = lon + 0.001
            };

            var location3 = new Location
            {
                Address = "Address 3",
                Latitude = requestPoint.Latitude + 0.002,
                Longitude = lon + 0.002
            };

            var locations = new List<Location> { location1, location2, location3 };

            var cluster = new Cluster
            {
                Locations = locations,
                Center = location2.Point,
                Boundary = LocationHelper.CreateBoundary(location2.Point),
                Id = Guid.NewGuid()
            };

            var clusters = new List<Cluster> { cluster };

            var expectedResult = new List<LocationResultView>();
            expectedResult.AddRange(locations.Select(location => new LocationResultView(location.Address,
                LocationHelper.CalculateDistance(location.Point, requestPoint), location.Latitude,
                location.Longitude)));
            expectedResult = expectedResult.Where(r => r.Distance <= request.MaxDistance).OrderBy(r => r.Distance)
                .Take(request.MaxResults).ToList();

            locationQueries.Setup(
                    x => x.GetClustersAsync())
                .Returns(Task.FromResult((IEnumerable<Cluster>)clusters));

            //Act
            var result = (await sut.Handle(request, default)).ToList();

            ////Assert
            result.Should().BeEquivalentTo(expectedResult);
            result.Should().HaveCount(expectedResult.Count);
            locationQueries.Verify(
                x => x.GetClustersAsync(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task
            Handle_CorrectRequestWithMaxDistanceAndMaxResultsConstraint_ReturnLocationsOnRange(
                [Frozen] Mock<ILocationQueries> locationQueries,
                GetLocationsQueryHandler sut)
        {
            //Arrange
            const double lat = 0.011;
            const double lon = 0.123;
            var requestPoint = new Point
            {
                Latitude = lat,
                Longitude = lon
            };
            var request = new GetLocationsQuery(requestPoint.Latitude, requestPoint.Longitude, 200, 1);

            var location1 = new Location
            {
                Address = "Address 1",
                Latitude = requestPoint.Latitude,
                Longitude = requestPoint.Longitude
            };

            var location2 = new Location
            {
                Address = "Address 2",
                Latitude = requestPoint.Latitude + 0.001,
                Longitude = requestPoint.Longitude + 0.001
            };

            var location3 = new Location
            {
                Address = "Address 3",
                Latitude = requestPoint.Latitude + 0.002,
                Longitude = requestPoint.Longitude + 0.002
            };

            var locations = new List<Location> { location1, location2, location3 };

            var cluster = new Cluster
            {
                Locations = locations,
                Center = location2.Point,
                Boundary = LocationHelper.CreateBoundary(location2.Point),
                Id = Guid.NewGuid()
            };

            var clusters = new List<Cluster> { cluster };

            var expectedResult = new List<LocationResultView>();
            expectedResult.AddRange(locations.Select(location => new LocationResultView(location.Address,
                LocationHelper.CalculateDistance(location.Point, requestPoint), location.Latitude,
                location.Longitude)));
            expectedResult = expectedResult.Where(r => r.Distance <= request.MaxDistance).OrderBy(r => r.Distance)
                .Take(request.MaxResults).ToList();

            locationQueries.Setup(
                    x => x.GetClustersAsync())
                .Returns(Task.FromResult((IEnumerable<Cluster>)clusters));

            //Act
            var result = (await sut.Handle(request, default)).ToList();

            ////Assert
            result.Should().BeEquivalentTo(expectedResult);
            result.Should().HaveCount(expectedResult.Count);
            locationQueries.Verify(
                x => x.GetClustersAsync(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task
            Handle_CorrectRequestMultiCluster_ReturnLocationsOnRange(
                [Frozen] Mock<ILocationQueries> locationQueries,
                GetLocationsQueryHandler sut)
        {
            //Arrange
            const double lat = 0.011;
            const double lon = 0.123;
            var requestPoint = new Point
            {
                Latitude = lat,
                Longitude = lon
            };
            var request = new GetLocationsQuery(requestPoint.Latitude, requestPoint.Longitude, 200000, 10);

            var location1 = new Location
            {
                Address = "Address 1",
                Latitude = requestPoint.Latitude,
                Longitude = requestPoint.Longitude
            };

            var location2 = new Location
            {
                Address = "Address 2",
                Latitude = requestPoint.Latitude + 0.001,
                Longitude = requestPoint.Longitude + 0.001
            };

            var location3 = new Location
            {
                Address = "Address 3",
                Latitude = requestPoint.Latitude + 0.002,
                Longitude = requestPoint.Longitude + 0.002
            };

            var locations1 = new List<Location> { location1, location2, location3 };

            var location4 = new Location
            {
                Address = "Address 1",
                Latitude = requestPoint.Latitude + 1,
                Longitude = requestPoint.Longitude
            };

            var location5 = new Location
            {
                Address = "Address 2",
                Latitude = requestPoint.Latitude + 1.001,
                Longitude = requestPoint.Longitude + 1.001
            };

            var location6 = new Location
            {
                Address = "Address 3",
                Latitude = requestPoint.Latitude + 1.002,
                Longitude = requestPoint.Longitude + 1.002
            };

            var locations2 = new List<Location> { location4, location5, location6 };

            var cluster1 = new Cluster
            {
                Locations = locations1,
                Center = location2.Point,
                Boundary = LocationHelper.CreateBoundary(location2.Point),
                Id = Guid.NewGuid()
            };

            var cluster2 = new Cluster
            {
                Locations = locations2,
                Center = location5.Point,
                Boundary = LocationHelper.CreateBoundary(location5.Point),
                Id = Guid.NewGuid()
            };

            var clusters = new List<Cluster> { cluster1, cluster2 };

            var expectedResult = new List<LocationResultView>();
            var resultViews1 = locations1.Select(location => new LocationResultView(location.Address,
                LocationHelper.CalculateDistance(location.Point, requestPoint), location.Latitude,
                location.Longitude));
            var resultViews2 = locations2.Select(location => new LocationResultView(location.Address,
                LocationHelper.CalculateDistance(location.Point, requestPoint), location.Latitude,
                location.Longitude));
            expectedResult.AddRange(resultViews1);
            expectedResult.AddRange(resultViews2);
            expectedResult = expectedResult.Where(r => r.Distance <= request.MaxDistance).OrderBy(r => r.Distance)
                .Take(request.MaxResults).ToList();

            locationQueries.Setup(
                    x => x.GetClustersAsync())
                .Returns(Task.FromResult((IEnumerable<Cluster>)clusters));

            //Act
            var result = (await sut.Handle(request, default)).ToList();

            ////Assert
            result.Should().BeEquivalentTo(expectedResult);
            result.Should().HaveCount(expectedResult.Count);
            locationQueries.Verify(
                x => x.GetClustersAsync(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task
            Handle_CorrectRequestNothingOnRange_ReturnNoLocations(
                [Frozen] Mock<ILocationQueries> locationQueries,
                GetLocationsQueryHandler sut)
        {
            //Arrange
            const double lat = 0.011;
            const double lon = 0.123;
            var requestPoint = new Point
            {
                Latitude = lat,
                Longitude = lon
            };
            var request = new GetLocationsQuery(requestPoint.Latitude, requestPoint.Longitude, 2000, 10);

            var location1 = new Location
            {
                Address = "Address 1",
                Latitude = requestPoint.Latitude + 2,
                Longitude = requestPoint.Longitude + 2
            };

            var location2 = new Location
            {
                Address = "Address 2",
                Latitude = requestPoint.Latitude + 2.001,
                Longitude = requestPoint.Longitude + 2.001
            };

            var location3 = new Location
            {
                Address = "Address 3",
                Latitude = requestPoint.Latitude + 2.002,
                Longitude = requestPoint.Longitude + 2.002
            };

            var locations1 = new List<Location> { location1, location2, location3 };

            var location4 = new Location
            {
                Address = "Address 1",
                Latitude = requestPoint.Latitude + 1,
                Longitude = requestPoint.Longitude
            };

            var location5 = new Location
            {
                Address = "Address 2",
                Latitude = requestPoint.Latitude + 1.001,
                Longitude = requestPoint.Longitude + 1.001
            };

            var location6 = new Location
            {
                Address = "Address 3",
                Latitude = requestPoint.Latitude + 1.002,
                Longitude = requestPoint.Longitude + 1.002
            };

            var locations2 = new List<Location> { location4, location5, location6 };

            var cluster1 = new Cluster
            {
                Locations = locations1,
                Center = location2.Point,
                Boundary = LocationHelper.CreateBoundary(location2.Point),
                Id = Guid.NewGuid()
            };

            var cluster2 = new Cluster
            {
                Locations = locations2,
                Center = location5.Point,
                Boundary = LocationHelper.CreateBoundary(location5.Point),
                Id = Guid.NewGuid()
            };

            var clusters = new List<Cluster> { cluster1, cluster2 };

            var expectedResult = new List<LocationResultView>();
            var resultViews1 = locations1.Select(location => new LocationResultView(location.Address,
                LocationHelper.CalculateDistance(location.Point, requestPoint), location.Latitude,
                location.Longitude));
            var resultViews2 = locations2.Select(location => new LocationResultView(location.Address,
                LocationHelper.CalculateDistance(location.Point, requestPoint), location.Latitude,
                location.Longitude));
            expectedResult.AddRange(resultViews1);
            expectedResult.AddRange(resultViews2);
            expectedResult = expectedResult.Where(r => r.Distance <= request.MaxDistance).OrderBy(r => r.Distance)
                .Take(request.MaxResults).ToList();

            locationQueries.Setup(
                    x => x.GetClustersAsync())
                .Returns(Task.FromResult((IEnumerable<Cluster>)clusters));

            //Act
            var result = (await sut.Handle(request, default)).ToList();

            ////Assert
            result.Should().BeEquivalentTo(expectedResult);
            result.Should().HaveCount(0);
            locationQueries.Verify(
                x => x.GetClustersAsync(), Times.Once);
        }
    }
}