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
        private readonly Dictionary<Guid, Cluster> _clusters = new();
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
            var initialClusters = request.Locations.Where(LocationHelper.IsValid)
                .Select(LocationHelper.CreateCluster).ToList();

            // TODO: I have to check for repeated addresses inside Redis
            for (var i = 0; i < initialClusters.Count; i++)
            {
                if (InCluster(initialClusters[i], out var match))
                {
                    match.Locations.Add(initialClusters[i].Locations.First());
                }
                else
                {
                    _clusters.Add(initialClusters[i].Id, initialClusters[i]);
                }
            }

            foreach (var cluster in _clusters)
            {
                await using var transaction =
                    await _unitOfWork.BeginTransactionAsync(cancellationToken);
                await _repository.CreateClusterAsync(cluster.Value);
                await _unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            }

            return Unit.Value;
        }

        private bool InCluster(Cluster cluster, out Cluster match)
        {
            match = null;
            foreach (var value in _clusters.Values)
            {
                if (LocationHelper.Overlaps(cluster.Center, value.Boundary))
                {
                    match = value;
                    return true;
                }
            }

            return false;
        }
    }
}