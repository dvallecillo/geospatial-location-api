using System;
using System.Threading;
using System.Threading.Tasks;

namespace GeospatialLocation.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<IDomainTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(
            IDomainTransaction? transaction, CancellationToken cancellationToken = default);
    }
}