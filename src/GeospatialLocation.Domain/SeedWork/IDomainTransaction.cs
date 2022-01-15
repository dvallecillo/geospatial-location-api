using System;

namespace GeospatialLocation.Domain.SeedWork
{
    public interface IDomainTransaction : IDisposable, IAsyncDisposable
    {
        Guid TransactionId { get; }
        bool Initial { get; }
        IDomainTransaction Clone();
    }
}