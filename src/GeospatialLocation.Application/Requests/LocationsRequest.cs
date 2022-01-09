namespace GeospatialLocation.Application
{
    public record LocationsRequest(
        double Lat, double Lon, int MaxDistance = 100, int MaxResults = 50);
}