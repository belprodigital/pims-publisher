using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Application.Adapters
{
    public interface ISynchronizationApi
    {
        Task<SynchronizationStatus> PostASynchronizationBatch(SynchronizationBatchEntity batch, CancellationToken cancellationToken);
    }
}
