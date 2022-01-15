namespace GeospatialLocation.Infrastructure.Redis.Helpers
{
    public static class KeyHelper
    {
        public const string GeospatialIndexCollectionKey = "geospatial-index-locations";
        public const string ClusterCollectionKey = "clusters";
        public const string ClusterDetailKey = "cluster:{0}";
    }
}