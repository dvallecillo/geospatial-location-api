using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeospatialLocation.Application.ViewModels;
using MediatR;

namespace GeospatialLocation.Application.Queries
{
    public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery,
        IEnumerable<LocationView>>
    {
        private readonly ILocationQueries queries;

        public GetLocationsQueryHandler(ILocationQueries queries)
        {
            this.queries = queries;
        }

        public async Task<IEnumerable<LocationView>> Handle(
            GetLocationsQuery request,
            CancellationToken cancellationToken)
        {
            return await queries.GetLocationsAsync(
                request.Lat,
                request.Lon,
                request.MaxDistance,
                request.MaxResults
            );
        }
    }
}