namespace GeospatialLocation.Application.Extensions
{
    public static class LocationExtensions
    {
        /// <summary>
        ///     Creates a new location that is <paramref name="offsetLat" />, <paramref name="offsetLon" /> meters from this
        ///     location.
        /// </summary>
        //public static Location Add(this Location location, double offsetLat, double offsetLon)
        //{
        //    var latitude = location.Latitude + offsetLat / 111111d;
        //    var longitude = location.Longitude + offsetLon / (111111d * Math.Cos(latitude));

        //    return new Location
        //    {
        //        Latitude = latitude,
        //        Longitude = longitude
        //    };
        //}

        /// <summary>
        ///     Calculates the distance between this location and another one, in meters.
        /// </summary>
        //public static double CalculateDistance(this Location thisLocation, Point targetLocation)
        //{
        //    var rlat1 = Math.PI * thisLocation.Latitude / 180;
        //    var rlat2 = Math.PI * targetLocation.Latitude / 180;
        //    //var rlon1 = Math.PI * location1.Longitude / 180;
        //    //var rlon2 = Math.PI * location2.Longitude / 180;
        //    var theta = thisLocation.Longitude - targetLocation.Longitude;
        //    var rtheta = Math.PI * theta / 180;
        //    var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
        //    dist = Math.Acos(dist);
        //    dist = dist * 180 / Math.PI;
        //    dist = dist * 60 * 1.1515;

        //    return dist * 1609.344;
        //}

        //public override string ToString()
        //{
        //    return null; //Latitude + ", " + Longitude;
        //}
    }
}