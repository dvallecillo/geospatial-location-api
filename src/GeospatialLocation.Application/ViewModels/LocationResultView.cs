namespace GeospatialLocation.Application.ViewModels
{
    public record LocationResultView(string Address, double? Distance, double Lat, double Lon);
}