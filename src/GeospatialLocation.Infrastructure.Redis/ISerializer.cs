namespace GeospatialLocation.Infrastructure.Redis
{
    public interface ISerializer
    {
        T? Deserialize<T>(byte[]? value);
        byte[] Serialize<T>(T? data);
    }
}