using System.Threading;
using System.Threading.Tasks;
using GeospatialLocation.Domain.Repositories;
using MediatR;

namespace GeospatialLocation.Application.Commands
{
    public class
        CreateRedisIndexLocationInitialLoadCommandHandler : IRequestHandler<CreateRedisIndexLocationInitialLoadCommand>
    {
        private readonly ILocationRepository _repository;

        public CreateRedisIndexLocationInitialLoadCommandHandler(
            ILocationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            CreateRedisIndexLocationInitialLoadCommand request, CancellationToken cancellationToken)
        {
            if (request.Locations == null)
            {
                return Unit.Value;
            }

            await _repository.InsertBulkGeospatialLocations(request.Locations);

            return Unit.Value;
        }
    }
}