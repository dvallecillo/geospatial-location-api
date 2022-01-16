using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeospatialLocation.Application.Constants;
using GeospatialLocation.Application.Exceptions;
using GeospatialLocation.Application.Exceptions.Errors;
using GeospatialLocation.Application.Helpers;
using GeospatialLocation.Application.ViewModels;
using GeospatialLocation.Domain.Models;
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
            GetLocationsQuery request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var clusters = (await _queries.GetClustersAsync()).ToList();

            if (clusters.Count == 0)
            {
                return null;
            }

            var requestPoint = new Point
            {
                Latitude = request.Lat,
                Longitude = request.Lon
            };

            var reachableClusters = clusters.Where(c =>
                LocationHelper.CalculateDistance(c.Center, requestPoint) <=
                request.MaxDistance + LocationConstants.ClusterDiagonal
            );

            var locations = reachableClusters.SelectMany(c => c.Locations);

            var results = new List<LocationResultView>();
            foreach (var location in locations)
            {
                var distance = LocationHelper.CalculateDistance(requestPoint, location.Point);

                if (distance <= request.MaxDistance)
                {
                    results.Add(new LocationResultView(location.Address, distance, location.Latitude,
                        location.Longitude));
                }
            }

            return results.OrderBy(r => r.Distance).Take(request.MaxResults);
        }

        private static void ValidateRequest(GetLocationsQuery request)
        {
            if (request.MaxDistance < 0)
            {
                throw new BusinessException(RequestErrors.NegativeMaxDistance);
            }

            if (request.MaxResults <= 0)
            {
                throw new BusinessException(RequestErrors.InvalidMaxResults);
            }

            if (!LocationHelper.IsValid(new Point
                {
                    Latitude = request.Lat,
                    Longitude = request.Lon
                }))
            {
                throw new BusinessException(RequestErrors.RequestLatLonOutOfRange);
            }
        }
    }
}