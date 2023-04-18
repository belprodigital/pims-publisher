using PimsPublisher.Domains;
using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Application.Adapters
{
    public interface ISynchronizationRepository : IRepository<SynchronizationAggregate>
    {
        Guid Add(SynchronizationAggregate synchronization);
        Task<SynchronizationAggregate?> GetSynchronization(Guid syncId, CancellationToken cancellationToken);
    }
}