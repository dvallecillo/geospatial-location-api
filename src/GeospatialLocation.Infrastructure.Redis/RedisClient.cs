using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeospatialLocation.Infrastructure.Redis
{
    public class RedisClient : IRedisClient
    {
        private readonly IRedisDataClient _dataClient;
        private readonly ISerializer _serializer;

        public RedisClient(IRedisDataClient dataClient, ISerializer serializer)
        {
            _dataClient = dataClient;
            _serializer = serializer;
        }

        public void AddToSet(string key, long id)
        {
            _dataClient.SetAddAsync(key, id);
        }

        public Task SetAsync<T>(string key, T data)
        {
            var bytes = _serializer.Serialize(data);
            return _dataClient.StringSetAsync(key, bytes);
        }

        public async Task<IEnumerable<T>> GetSortedAsync<T>(string key, string get)
        {
            var datas = await _dataClient.SortAsync(key, get);
            return datas.Select(bytes => _serializer.Deserialize<T>(bytes));
        }
    }
}