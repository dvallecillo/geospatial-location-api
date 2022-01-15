using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeospatialLocation.Application.ViewModels;
using MediatR;

namespace GeospatialLocation.Application.Queries
{
    public class GetRedisIndexLocationsQueryHandler : IRequestHandler<GetRedisIndexLocationsQuery,
        IEnumerable<LocationResultView>>
    {
        private readonly ILocationQueries _queries;

        public GetRedisIndexLocationsQueryHandler(ILocationQueries queries)
        {
            _queries = queries;
        }

        public async Task<IEnumerable<LocationResultView>> Handle(
            GetRedisIndexLocationsQuery request,
            CancellationToken cancellationToken)
        {
            return await _queries.GetRedisIndexLocationsAsync(
                request.Lat,
                request.Lon,
                request.MaxDistance,
                request.MaxResults
            );
        }
    }
}