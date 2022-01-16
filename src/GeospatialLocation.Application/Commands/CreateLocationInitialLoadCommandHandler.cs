using System;
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

            // TODO: Distinct by address
            var initialClusters = request.Locations.Where(l => LocationHelper.IsValid(l.Point))
                .Select(LocationHelper.CreateCluster).ToList();

            Dictionary<Guid, Cluster> clustersToInsert = new();
            // TODO: Check for repeated addresses inside Redis
            foreach (var initialCluster in initialClusters)
            {
                if (InCluster(clustersToInsert.Values, initialCluster, out var match))
                {
                    match.Locations.Add(initialCluster.Locations.First());
                }
                else
                {
                    clustersToInsert.Add(initialCluster.Id, initialCluster);
                }
            }

            foreach (var cluster in clustersToInsert)
            {
                await using var transaction =
                    await _unitOfWork.BeginTransactionAsync(cancellationToken);
                await _repository.CreateClusterAsync(cluster.Value);
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