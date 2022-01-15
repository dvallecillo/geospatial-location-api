using GeospatialLocation.Domain.Models;

namespace GeospatialLocation.Domain.Entities
{
    public class Location
    {
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Point Point => new()
        {
            Latitude = Latitude,
            Longitude = Longitude
        };
    }
}