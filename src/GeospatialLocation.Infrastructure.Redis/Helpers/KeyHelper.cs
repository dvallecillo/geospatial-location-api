namespace GeospatialLocation.Infrastructure.Redis.Helpers
{
    public static class KeyHelper
    {
        public const string ClusterCollectionKey = "clusters";
        public const string ClusterDetailKey = "cluster:{0}";
        public const string ClusterCenterKey = "cluster:{0}:center";
    }
}