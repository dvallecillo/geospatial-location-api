using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GeospatialLocation.API.ExampleData;
using GeospatialLocation.Application.Commands;
using GeospatialLocation.Application.Queries;
using GeospatialLocation.Application.Requests;
using GeospatialLocation.Application.ViewModels;
using GeospatialLocation.Domain.Entities;
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
            var query = new GetLocationsQuery(request.Lat, request.Lon,
                request.MaxDistance, request.MaxResults);

            var locations = await _bus.Send(query);
            return Ok(locations);
        }

        //TODO: move it to the startup
        [HttpPost]
        public async Task<IActionResult> CreateInitialLoad()
        {
            ICollection<Location> records;
            try
            {
                records = ExampleDataReader.Read();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            var command = new CreateLocationInitialLoadCommand(records);

            await _bus.Send(command);

            return Ok();
        }
    }
}