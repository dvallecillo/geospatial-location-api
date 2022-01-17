using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace GeospatialLocation.Infrastructure.Redis
{
    public class RedisDataClient : IRedisDataClient
    {
        private static readonly Lazy<ConnectionMultiplexer> Connection
            = new(RedisConnectionFactory.CreateConnection);

        private readonly IDatabase _database;
        private ITransaction? _transaction;

        public RedisDataClient()
        {
            _database = Redis.GetDatabase();
        }

        public ConnectionMultiplexer Redis => Connection.Value;

        public ITransaction CreateTransaction()
        {
            _transaction = _database.CreateTransaction();
            return _transaction;
        }

        public Task StringSetAsync(string key, byte[] bytes)
        {
            return Task.FromResult(
                _transaction != null
                    ? _transaction.StringSetAsync(key, bytes)
                    : _database.StringSetAsync(key, bytes));
        }

        public async Task<byte[][]> SortAsync(string key, string get)
        {
            var members = await _database.SortAsync(key, 0, -1, Order.Ascending, SortType.Alphabetic, default,
                new RedisValue[] { get });

            return members.Select(x => (byte[])x).ToArray();
        }

        public void Dispose()
        {
            _transaction = null;
            GC.SuppressFinalize(this);
        }

        public Task SetAddAsync(string key, ICollection<long> collection)
        {
            var values = collection.Select(v => (RedisValue)v);
            return _transaction != null
                ? _transaction.SetAddAsync(key, values.ToArray())
                : _database.SetAddAsync(key, values.ToArray());
        }
    }
}