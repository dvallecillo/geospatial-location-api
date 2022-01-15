using System;
using System.Threading;
using System.Threading.Tasks;
using GeospatialLocation.Domain.SeedWork;

namespace GeospatialLocation.Infrastructure.Redis
{
    public class RedisUnitOfWork : IUnitOfWork
    {
        private readonly IRedisDataClient _redisDataClient;
        private RedisTransaction? _currentTransaction;

        public RedisUnitOfWork(IRedisDataClient redisDataClient)
        {
            _redisDataClient = redisDataClient;
        }

        public Task<IDomainTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                return Task.FromResult(_currentTransaction.Clone());
            }

            _currentTransaction =
                new RedisTransaction(_redisDataClient.CreateTransaction());

            return Task.FromResult((IDomainTransaction)_currentTransaction);
        }

        public async Task CommitTransactionAsync(
            IDomainTransaction? transaction,
            CancellationToken cancellationToken = default)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            _currentTransaction ??=
                new RedisTransaction(_redisDataClient.CreateTransaction());

            if (transaction.TransactionId != _currentTransaction.TransactionId)
            {
                throw new InvalidOperationException(
                    $"Transaction {transaction.TransactionId} is not current");
            }

            if (!transaction.Initial)
            {
                return;
            }

            try
            {
                if (_currentTransaction.Transaction != null)
                {
                    await _currentTransaction.Transaction.ExecuteAsync();
                }
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            _redisDataClient.Dispose();

            if (_currentTransaction == null)
            {
                return;
            }

            _currentTransaction.Dispose();
            _currentTransaction = null;

            GC.SuppressFinalize(this);
        }
    }
}