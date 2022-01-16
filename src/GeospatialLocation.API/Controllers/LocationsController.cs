using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GeospatialLocation.Application.Queries;
using GeospatialLocation.Application.Requests;
using GeospatialLocation.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeospatialLocation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly IMediator _bus;

        public LocationsController(IMediator bus)
        {
            _bus = bus;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LocationResultView>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<LocationResultView>>> Get(
            [FromQuery] LocationsRequest request)
        {
            var (requestPoint, maxDistance, maxResults) = request;
            if (requestPoint == null)
            {
                throw new ArgumentNullException();
            }

            var query = new GetLocationsQuery(requestPoint.Latitude, requestPoint.Longitude,
                maxDistance, maxResults);

            var locations = await _bus.Send(query);
            return Ok(locations);
        }
    }
}