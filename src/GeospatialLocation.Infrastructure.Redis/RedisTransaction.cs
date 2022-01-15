using System;
using System.Threading.Tasks;
using GeospatialLocation.Domain.SeedWork;
using StackExchange.Redis;

namespace GeospatialLocation.Infrastructure.Redis
{
    public class RedisTransaction : IDomainTransaction
    {
        public RedisTransaction(ITransaction? transaction)
        {
            TransactionId = Guid.NewGuid();
            Initial = true;
            Transaction = transaction;
        }

        public ITransaction? Transaction { get; init; }

        public Guid TransactionId { get; init; }
        public bool Initial { get; init; }

        public IDomainTransaction Clone()
        {
            return new RedisTransaction(null)
            {
                TransactionId = TransactionId,
                Initial = false
            };
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return new ValueTask();
        }
    }
}