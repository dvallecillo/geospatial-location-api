using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using GeospatialLocation.Application;
using GeospatialLocation.Application.Commands;
using GeospatialLocation.Application.Queries;
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
        public async Task<ActionResult<IEnumerable<LocationResultView>>> Get(
            [FromQuery] LocationsRequest request)
        {
            var query = new GetLocationsQuery(request.Lat, request.Lon,
                request.MaxDistance, request.MaxResults);

            var locations = await _bus.Send(query);
            return Ok(locations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInitialLoad([FromQuery] string fileName)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var path = Path.Combine(currentDirectory, $"ExampleData\\{fileName}.csv");

            IEnumerable<Location> records;
            try
            {
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    records = csv.GetRecords<Location>().ToList();
                }
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