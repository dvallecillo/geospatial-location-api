using System.Collections.Generic;
using GeospatialLocation.Domain.Entities;
using MediatR;

namespace GeospatialLocation.Application.Commands
{
    public record CreateRedisIndexLocationInitialLoadCommand(
        ICollection<Location> Locations) : IRequest;
}