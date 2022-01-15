using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeospatialLocation.Application.Constants;
using GeospatialLocation.Application.Helpers;
using GeospatialLocation.Domain.Entities;
using GeospatialLocation.Domain.Models;
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
                .Select(CreateInitialCluster).ToList();

            // TODO: I have check for repeated addresses inside Redis
            for (var i = 0; i < initialClusters.Count; i++)
            {
                if (InCluster(initialClusters[i], out var match))
                {
                    LocationHelper.CombineCluster(initialClusters[i], match);
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

        private static Cluster CreateInitialCluster(Location location)
        {
            var latitudesOffset = new List<double>
            {
                LocationHelper.Add(location.Point, LocationConstants.ClusterRange, 0).Latitude,
                LocationHelper.Add(location.Point, -1 * LocationConstants.ClusterRange, 0).Latitude
            };
            var longitudesOffset = new List<double>
            {
                LocationHelper.Add(location.Point, 0, LocationConstants.ClusterRange).Longitude,
                LocationHelper.Add(location.Point, 0, -1 * LocationConstants.ClusterRange).Longitude
            };

            var boundary = new Boundary
            {
                MinLat = latitudesOffset.Min(),
                MaxLat = latitudesOffset.Max(),
                MinLon = longitudesOffset.Min(),
                MaxLon = longitudesOffset.Max()
            };
            var center = new Point
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
            return new Cluster
            {
                Id = Guid.NewGuid(),
                Locations = new List<Location>
                {
                    location
                },
                Center = center,
                Boundary = boundary
            };
        }
    }
}