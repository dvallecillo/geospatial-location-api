using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace GeospatialLocation.Infrastructure.Redis
{
    public class RedisDataClient : IRedisDataClient
    {
        private static readonly Lazy<ConnectionMultiplexer> Connection
            = new(RedisConnectionFactory.CreateConnection);

        private readonly IDatabase database;

        public RedisDataClient()
        {
            database = Redis.GetDatabase();
        }

        public ConnectionMultiplexer Redis => Connection.Value;

        public async Task<byte[]> StringGetAsync(string key)
        {
            return await database.StringGetAsync(key);
        }

        public Task StringSetAsync(string key, byte[] bytes)
        {
            return Task.FromResult(database.StringSetAsync(key, bytes));
        }

        public Task StringSetAsync(string key, string value)
        {
            return Task.FromResult(database.StringSetAsync(key, value));
        }

        public Task<long> IncrementAsync(string key)
        {
            return database.StringIncrementAsync(key);
        }

        public Task SetAddAsync(string key, long id)
        {
            return database.SetAddAsync(key, id);
        }

        public Task SetRemoveAsync(string key, long id)
        {
            return database.SetRemoveAsync(key, id);
        }

        public Task<bool> ExistsAsync(string key)
        {
            return database.KeyExistsAsync(key);
        }

        public Task HashSetAsync(string key, string hash, byte[] value)
        {
            return database.HashSetAsync(key, hash, value);
        }

        public async Task<byte[]?> HashGetAsync(string key, string field)
        {
            var result = await database.HashGetAsync(key, field);
            return result;
        }

        public async Task<long[]> GetSetAsIntAsync(string key)
        {
            var members = await database.SetMembersAsync(key);
            var result = new long[members.Length];

            var i = 0;
            foreach (var member in members)
            {
                result[i++] = (long)member;
            }

            return result;
        }

        public Task HashDeleteAsync(string key, string field)
        {
            return database.HashDeleteAsync(key, field);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}