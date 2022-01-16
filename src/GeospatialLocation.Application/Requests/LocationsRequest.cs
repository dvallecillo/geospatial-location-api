namespace GeospatialLocation.Application.Requests
{
#nullable enable
    public record LocationsRequest(
        PointRequest Location, int MaxDistance = 100, int MaxResults = 50);
}