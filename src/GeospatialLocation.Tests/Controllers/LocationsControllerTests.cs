using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using GeospatialLocation.API.Controllers;
using GeospatialLocation.Application.Queries;
using GeospatialLocation.Application.Requests;
using GeospatialLocation.Application.ViewModels;
using GeospatialLocation.Tests.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GeospatialLocation.Tests.Controllers
{
    public class LocationsControllerTests
    {
        /// <summary>
        ///     Check controller flow to throw an exception when null location is requested
        /// </summary>
        [Theory]
        [AutoMoqData]
        public async Task
            Controller_NullLocation_ThrowException(
                int maxDistance,
                int maxResults,
                [Frozen] Mock<IMediator> bus)
        {
            //Arrange
            var sut = new LocationsController(bus.Object);
            var request = new LocationsRequest(null, maxDistance, maxResults);

            //Act + Assert
            Func<Task> act = () => sut.Get(request);
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage(new ArgumentNullException().Message);

            bus.Verify(x => x.Send(It.IsAny<GetLocationsQuery>(), default), Times.Never);
        }

        /// <summary>
        ///     Check controller normal flow and response
        /// </summary>
        [Theory]
        [AutoMoqData]
        public async Task
            Controller_CorrectRequest_ReturnOk(
                LocationsRequest request,
                IEnumerable<LocationResultView> locations,
                [Frozen] Mock<IMediator> bus)
        {
            //Arrange
            var sut = new LocationsController(bus.Object);
            bus.Setup(x => x.Send(It.IsAny<GetLocationsQuery>(), default))
                .Returns(Task.FromResult(locations));

            // Act
            var result = await sut.Get(request);

            // Assert
            bus.Verify(x => x.Send(It.IsAny<GetLocationsQuery>(), default), Times.Once);
            Assert.IsType<ActionResult<IEnumerable<LocationResultView>>>(result);
            var actionResult = result.Result as OkObjectResult;
            actionResult.Should().NotBeNull();
            actionResult.StatusCode.Should().Be(200);
        }
    }
}