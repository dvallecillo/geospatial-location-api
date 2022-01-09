using System.Threading.Tasks;

namespace GeospatialLocation.Infrastructure.Redis
{
    public class RedisClient : IRedisClient
    {
        private readonly IRedisDataClient dataClient;
        private readonly IScalarSerializer scalarSerializer;
        private readonly ISerializer serializer;

        public RedisClient(
            ISerializer serializer, IRedisDataClient dataClient, IScalarSerializer scalarSerializer)
        {
            this.serializer = serializer;
            this.dataClient = dataClient;
            this.scalarSerializer = scalarSerializer;
        }

        public Task<long> GenerateIdAsync(string key)
        {
            return dataClient.IncrementAsync(key);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var bytes = await dataClient.StringGetAsync(key);
            var data = serializer.Deserialize<T>(bytes);

            return data;
        }

        public Task SetAsync<T>(string key, T data)
        {
            var bytes = serializer.Serialize(data);
            return dataClient.StringSetAsync(key, bytes);
        }

        public Task SetStringAsync(string key, string value)
        {
            return dataClient.StringSetAsync(key, value);
        }

        public Task<bool> ExistsAsync(string key)
        {
            return dataClient.ExistsAsync(key);
        }

        public Task HashSetAsync(string key, string field, long value)
        {
            var bytes = scalarSerializer.Serialize(value);

            return dataClient.HashSetAsync(key, field, bytes);
        }

        public async Task<long?> HashGetAsync(string key, string field)
        {
            var result = await dataClient.HashGetAsync(key, field);

            if (result is null)
            {
                return null;
            }

            return scalarSerializer.Deserialize<long>(result);
        }

        public Task<long[]> GetSetAsIntAsync(string key)
        {
            return dataClient.GetSetAsIntAsync(key);
        }

        public Task AddToSetAsync(string key, long id)
        {
            return dataClient.SetAddAsync(key, id);
        }

        public Task RemoveFromSetAsync(string key, long id)
        {
            return dataClient.SetRemoveAsync(key, id);
        }

        public Task HashSetScalarAsync<T>(string key, string field, T value)
        {
            var bytes = scalarSerializer.Serialize(value);
            return dataClient.HashSetAsync(key, field, bytes);
        }

        public Task HashDeleteAsync(string key, string field)
        {
            return dataClient.HashDeleteAsync(key, field);
        }
    }
}