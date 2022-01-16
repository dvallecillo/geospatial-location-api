namespace GeospatialLocation.Application.Exceptions.Errors
{
    public class RequestErrors
    {
        public static ErrorCode RequestLatLonOutOfRange = new(
            "LAT_LON_OUT_OF_RANGE", "The provided lat/lon coordinates are out of valid range");

        public static ErrorCode NegativeMaxDistance = new(
            "NEGATIVE_MAX_DISTANCE", "The provided maximum distance is a negative number");

        public static ErrorCode InvalidMaxResults = new(
            "INVALID_MAX_RESULTS", "The provided maximum results is not a positive number");
    }
}