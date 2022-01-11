namespace GeospatialLocation.Infrastructure.Redis
{
    public abstract class RedisRepository
    {
        protected RedisRepository(IRedisClient client)
        {
            Client = client;
        }

        public IRedisClient Client { get; }
    }
}