using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeospatialLocation.Application.Queries;
using GeospatialLocation.Application.ViewModels;
using MediatR;

namespace GeospatialLocation.Infrastructure.Redis.Queries
{
    public class LocationQueries : RedisRepository, ILocationQueries
    {
        private readonly IMediator bus;

        public LocationQueries(IRedisClient client, IMediator bus) : base(client)
        {
            this.bus = bus;
        }

        public Task<IEnumerable<LocationView>> GetLocationsAsync(double lat, double lon, int maxDistance,
            int maxResults)
        {
            throw new NotImplementedException();
        }
    }
}