using Microsoft.Identity.Abstractions;
using PimsPublisher.Domains.Entities;
using PimsPublisher.Application.Adapters;
using PimsPublisher.Infrastructure.K3dClients.DataContract;

namespace PimsPublisher.Infrastructure.K3dClients
{
    public class SynchronizationApi : ISynchronizationApi
    {

        private readonly IDownstreamApi api;
        public SynchronizationApi(IDownstreamApi downstreamApi)
        {
            api = downstreamApi ?? throw new ArgumentNullException(nameof(downstreamApi));
        }


        public async Task<Domains.Entities.SynchronizationStatus> PostASynchronizationBatch(SynchronizationBatchEntity batch, CancellationToken cancellationToken)
        {
            var status = await api.PostForAppAsync<SynchSessionBatching, DataContract.SynchronizationStatus>("K3dApim", batch.ToContractModel());

            return status?.ToSynchronizationStatusEntity() ?? new Domains.Entities.SynchronizationStatus();
        }
    }
}
