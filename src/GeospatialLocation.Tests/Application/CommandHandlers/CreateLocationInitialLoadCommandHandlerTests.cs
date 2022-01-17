using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using GeospatialLocation.Application.Commands;
using GeospatialLocation.Domain.Entities;
using GeospatialLocation.Domain.Repositories;
using GeospatialLocation.Domain.SeedWork;
using GeospatialLocation.Tests.Utils;
using Moq;
using Xunit;

namespace GeospatialLocation.Tests.Application.CommandHandlers
{
    public class CreateLocationInitialLoadCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task
            Handle_CorrectData_CreatesClusters(
                [Frozen] Mock<ILocationRepository> locationRepository,
                [Frozen] Mock<IUnitOfWork> unitOfWork,
                [Frozen] Mock<IDomainTransaction> domainTransaction,
                CreateLocationInitialLoadCommandHandler sut)
        {
            //Arrange
            const double lat1 = 1.011;
            const double lon1 = 3.123;

            var location1 = new Location
            {
                Address = "Address 1",
                Latitude = lat1,
                Longitude = lon1
            };

            var location2 = new Location
            {
                Address = "Address 2",
                Latitude = lat1 + 1,
                Longitude = lon1 + 1
            };

            var location3 = new Location
            {
                Address = "Address 3",
                Latitude = lat1 + 2,
                Longitude = lon1 + 2
            };

            var locations = new List<Location> { location1, location2, location3 };

            var command = new CreateLocationInitialLoadCommand(locations);


            unitOfWork.Setup(
                x =>
                    x.BeginTransactionAsync(default)
            ).Returns(Task.FromResult(domainTransaction.Object));

            locationRepository.Setup(
                x =>
                    x.CreateClustersAsync(It.IsAny<ICollection<Cluster>>())
            ).Returns(Task.CompletedTask);


            unitOfWork.Setup(
                x =>
                    x.CommitTransactionAsync(null, default)
            ).Returns(Task.CompletedTask);

            //Act
            await sut.Handle(command, default);

            //Assert
            locationRepository.Verify(x => x.CreateClustersAsync(It.IsAny<ICollection<Cluster>>()),
                Times.Once());
        }

        [Theory]
        [AutoMoqData]
        public async Task
            Handle_InvalidData_CreatesNoClusters(
                [Frozen] Mock<ILocationRepository> locationRepository,
                [Frozen] Mock<IUnitOfWork> unitOfWork,
                [Frozen] Mock<IDomainTransaction> domainTransaction,
                CreateLocationInitialLoadCommandHandler sut)
        {
            //Arrange
            const double lat1 = 11111.011;
            const double lon1 = 31111.123;

            var location1 = new Location
            {
                Address = "Address 1",
                Latitude = lat1,
                Longitude = lon1
            };

            var location2 = new Location
            {
                Address = "Address 2",
                Latitude = lat1 + 1,
                Longitude = lon1 + 1
            };

            var location3 = new Location
            {
                Address = "Address 3",
                Latitude = lat1 + 2,
                Longitude = lon1 + 2
            };

            var locations = new List<Location> { location1, location2, location3 };

            var command = new CreateLocationInitialLoadCommand(locations);


            unitOfWork.Setup(
                x =>
                    x.BeginTransactionAsync(default)
            ).Returns(Task.FromResult(domainTransaction.Object));

            locationRepository.Setup(
                x =>
                    x.CreateClustersAsync(It.IsAny<ICollection<Cluster>>())
            ).Returns(Task.CompletedTask);


            unitOfWork.Setup(
                x =>
                    x.CommitTransactionAsync(null, default)
            ).Returns(Task.CompletedTask);

            //Act
            await sut.Handle(command, default);

            //Assert
            locationRepository.Verify(x => x.CreateClustersAsync(It.IsAny<ICollection<Cluster>>()),
                Times.Never());
        }

        [Theory]
        [AutoMoqData]
        public async Task
            Handle_ClusteredData_CreatesCorrectCluster(
                [Frozen] Mock<ILocationRepository> locationRepository,
                [Frozen] Mock<IUnitOfWork> unitOfWork,
                [Frozen] Mock<IDomainTransaction> domainTransaction,
                CreateLocationInitialLoadCommandHandler sut)
        {
            //Arrange
            const double lat1 = 11.011;
            const double lon1 = 11.123;

            var location1 = new Location
            {
                Address = "Address 1",
                Latitude = lat1,
                Longitude = lon1
            };

            var location2 = new Location
            {
                Address = "Address 2",
                Latitude = lat1 + 0.0001,
                Longitude = lon1 + 1
            };

            var location3 = new Location
            {
                Address = "Address 3",
                Latitude = lat1 + 0.0002,
                Longitude = lon1 + 0.0002
            };

            var locations = new List<Location> { location1, location2, location3 };

            var command = new CreateLocationInitialLoadCommand(locations);


            unitOfWork.Setup(
                x =>
                    x.BeginTransactionAsync(default)
            ).Returns(Task.FromResult(domainTransaction.Object));

            locationRepository.Setup(
                x =>
                    x.CreateClustersAsync(It.IsAny<ICollection<Cluster>>())
            ).Returns(Task.CompletedTask);


            unitOfWork.Setup(
                x =>
                    x.CommitTransactionAsync(null, default)
            ).Returns(Task.CompletedTask);

            //Act
            await sut.Handle(command, default);

            //Assert
            locationRepository.Verify(x => x.CreateClustersAsync(It.IsAny<ICollection<Cluster>>()),
                Times.Once());
        }
    }
}