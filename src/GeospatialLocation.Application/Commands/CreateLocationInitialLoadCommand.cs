using System.Collections.Generic;
using GeospatialLocation.Domain.Entities;
using MediatR;

namespace GeospatialLocation.Application.Commands
{
    public record CreateLocationInitialLoadCommand(
        ICollection<Location> Locations) : IRequest;
}