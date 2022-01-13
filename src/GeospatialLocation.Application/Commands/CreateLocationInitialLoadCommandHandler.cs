using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeospatialLocation.Application.Services;
using GeospatialLocation.Domain.Repositories;
using MediatR;

namespace GeospatialLocation.Application.Commands
{
    public class CreateLocationInitialLoadCommandHandler : IRequestHandler<CreateLocationInitialLoadCommand>
    {
        private readonly ILocationRepository _repository;
        private readonly ILocationService _service;

        public CreateLocationInitialLoadCommandHandler(
            ILocationRepository repository, ILocationService service)
        {
            _repository = repository;
            _service = service;
        }

        public async Task<Unit> Handle(
            CreateLocationInitialLoadCommand request, CancellationToken cancellationToken)
        {
            if (request.Locations == null)
            {
                return Unit.Value;
            }

            var locations = request.Locations.ToList();

            _service.BulkLoadKdTree(locations);

            await _repository.InsertBulkLocations(locations);

            return Unit.Value;
        }
    }
}