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
            if (request.Location == null)
            {
                throw new ArgumentNullException();
            }

            var query = new GetLocationsQuery(request.Location.Latitude, request.Location.Longitude,
                request.MaxDistance, request.MaxResults);

            var locations = await _bus.Send(query);
            return Ok(locations);
        }
    }
}