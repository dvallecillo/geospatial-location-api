using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeospatialLocation.Application.ViewModels;
using MediatR;

namespace GeospatialLocation.Application.Queries
{
    public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery,
        IEnumerable<LocationResultView>>
    {
        private readonly ILocationQueries _queries;

        public GetLocationsQueryHandler(ILocationQueries queries)
        {
            _queries = queries;
        }

        public async Task<IEnumerable<LocationResultView>> Handle(
            GetLocationsQuery request,
            CancellationToken cancellationToken)
        {
            return await _queries.GetLocationsAsync(
                request.Lat,
                request.Lon,
                request.MaxDistance,
                request.MaxResults
            );
        }
    }
}