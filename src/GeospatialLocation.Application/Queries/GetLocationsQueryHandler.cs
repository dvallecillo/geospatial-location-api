using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeospatialLocation.Application.Constants;
using GeospatialLocation.Application.Helpers;
using GeospatialLocation.Application.ViewModels;
using GeospatialLocation.Domain.Entities;
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
            var clusters = (await _queries.GetClustersAsync(
                request.Lat,
                request.Lon,
                request.MaxDistance,
                request.MaxResults
            )).ToList();

            if (clusters.Count == 0)
            {
                return null;
            }

            var requestPoint = new Point
            {
                Latitude = request.Lat,
                Longitude = request.Lon
            };

            var distances = new Dictionary<Cluster, double>();
            foreach (var cluster in clusters)
            {
                var distance = LocationHelper.CalculateDistance(cluster.Center, requestPoint);
                distances[cluster] = distance;
            }

            // TODO: Smulwereld; Kastanjehof 26, Maasland	51.9371305	4.2704511
            var reachableClusters = clusters.Where(c =>
                LocationHelper.CalculateDistance(c.Center, requestPoint) <=
                request.MaxDistance + LocationConstants.ClusterDiagonal
            );

            var locations = reachableClusters.SelectMany(c => c.Locations).ToList();

            var results = new List<LocationResultView>();
            foreach (var location in locations)
            {
                var point = new Point
                {
                    Latitude = request.Lat,
                    Longitude = request.Lon
                };
                var distance = LocationHelper.CalculateDistance(point, location.Point);

                if (distance <= request.MaxDistance)
                {
                    results.Add(new LocationResultView(location.Address, distance, point.Latitude, point.Longitude));
                }
            }

            var locationResultViews = results.OrderBy(r => r.Distance).Take(request.MaxResults);

            return locationResultViews;
        }
    }
}