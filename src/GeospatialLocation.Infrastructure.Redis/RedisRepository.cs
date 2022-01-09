namespace GeospatialLocation.Infrastructure.Redis
{
    public abstract class RedisRepository
    {
        protected RedisRepository(IRedisClient client)
        {
            Client = client;
        }

        public IRedisClient Client { get; }

        protected void AddToSets(string set, long id)
        {
            Client.AddToSetAsync(set, id);
        }

        protected void RemoveFromSets(string set, long id)
        {
            Client.RemoveFromSetAsync(set, id);
        }

        protected void HashSet(string key, string field, long id)
        {
            Client.HashSetAsync(key, field, id);
        }

        protected void Set<T>(string key, T data)
        {
            Client.SetAsync(key, data);
        }

        protected void SetString(string key, string value)
        {
            Client.SetStringAsync(key, value);
        }

        protected void HashDelete(string key, string field)
        {
            Client.HashDeleteAsync(key, field);
        }
    }
}