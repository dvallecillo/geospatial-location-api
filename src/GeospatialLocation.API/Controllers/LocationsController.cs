using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Application;
using GeospatialLocation.Application.Queries;
using GeospatialLocation.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeospatialLocation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationsController : ControllerBase
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        private readonly IMediator bus;

        public LocationsController(IMediator bus)
        {
            this.bus = bus;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> GetWeather()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //        {
        //            Date = DateTime.Now.AddDays(index),
        //            TemperatureC = rng.Next(-20, 55),
        //            Summary = Summaries[rng.Next(Summaries.Length)]
        //        })
        //        .ToArray();
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationView>>> Get(
            [FromQuery] LocationsRequest request)
        {
            var query = new GetLocationsQuery(request.Lat, request.Lon,
                request.MaxDistance, request.MaxResults);

            var locations = await bus.Send(query);
            return Ok(locations);
        }
    }
}