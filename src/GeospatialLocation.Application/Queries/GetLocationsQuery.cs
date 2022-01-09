using System.Collections.Generic;
using GeospatialLocation.Application.ViewModels;
using MediatR;

namespace GeospatialLocation.Application.Queries
{
    public record GetLocationsQuery
        (double Lat, double Lon, int MaxDistance, int MaxResults) :
            IRequest<IEnumerable<LocationView>>;
}