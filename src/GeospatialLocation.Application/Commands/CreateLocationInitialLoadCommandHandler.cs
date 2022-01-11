using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeospatialLocation.Domain.Repositories;
using MediatR;

namespace GeospatialLocation.Application.Commands
{
    public class CreateLocationInitialLoadCommandHandler : IRequestHandler<CreateLocationInitialLoadCommand>
    {
        private readonly ILocationRepository repository;

        public CreateLocationInitialLoadCommandHandler(
            ILocationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(
            CreateLocationInitialLoadCommand request, CancellationToken cancellationToken)
        {
            if (request.Locations == null)
            {
                return Unit.Value;
            }

            await repository.InsertBulkLocations(request.Locations.ToList());

            return Unit.Value;
        }
    }
}