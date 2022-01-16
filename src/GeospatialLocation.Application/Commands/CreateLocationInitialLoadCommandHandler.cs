using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeospatialLocation.Application.Helpers;
using GeospatialLocation.Domain.Entities;
using GeospatialLocation.Domain.Repositories;
using GeospatialLocation.Domain.SeedWork;
using MediatR;

namespace GeospatialLocation.Application.Commands
{
    public class
        CreateLocationInitialLoadCommandHandler : IRequestHandler<CreateLocationInitialLoadCommand>
    {
        private readonly ILocationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLocationInitialLoadCommandHandler(
            ILocationRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(
            CreateLocationInitialLoadCommand request, CancellationToken cancellationToken)
        {
            if (request.Locations == null)
            {
                return Unit.Value;
            }

            var initialClusters = request.Locations.GroupBy(x => new { x.Latitude, x.Longitude, x.Address }).Select(g =>
                    new Location
                    {
                        Address = g.Key.Address,
                        Latitude = g.Key.Latitude,
                        Longitude = g.Key.Longitude
                    })
                .Where(l => LocationHelper.IsValid(l.Point))
                .Select(LocationHelper.CreateCluster).ToList();

            List<Cluster> clustersToInsert = new();

            foreach (var initialCluster in initialClusters)
            {
                if (InCluster(clustersToInsert, initialCluster, out var match))
                {
                    match.Locations.Add(initialCluster.Locations.First());
                }
                else
                {
                    clustersToInsert.Add(initialCluster);
                }
            }

            foreach (var cluster in clustersToInsert)
            {
                await using var transaction =
                    await _unitOfWork.BeginTransactionAsync(cancellationToken);
                await _repository.CreateClusterAsync(cluster);
                await _unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            }

            return Unit.Value;
        }

        private static bool InCluster(IEnumerable<Cluster> currentClusters, Cluster cluster, out Cluster match)
        {
            match = null;
            foreach (var value in currentClusters)
            {
                if (LocationHelper.InsideBoundary(cluster.Center, value.Boundary))
                {
                    match = value;
                    return true;
                }
            }

            return false;
        }
    }
}