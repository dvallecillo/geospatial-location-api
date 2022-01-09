using System;
using GeospatialLocation.Domain.SeedWork;

namespace GeospatialLocation.Infrastructure.Redis
{
    public class RedisUnitOfWork : IUnitOfWork
    {
        private readonly IRedisDataClient redisDataClient;

        public RedisUnitOfWork(IRedisDataClient redisDataClient)
        {
            this.redisDataClient = redisDataClient;
        }


        public void Dispose()
        {
            redisDataClient.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}