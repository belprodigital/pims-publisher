using PimsPublisher.Application.Adapters;
using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Application.Services
{
    public class SynchronizationService
    {
        private readonly IPimsAttributesDataService _pimsAttributesDataService;
        private readonly ISynchronizationApi _synchronizationApi;
        private readonly ISynchronizationRepository _synchronizationRepository;

        public SynchronizationService(IPimsAttributesDataService pimsAttributesDataService, ISynchronizationApi synchronizationApi, ISynchronizationRepository synchronizationRepository)
        {
            _pimsAttributesDataService = pimsAttributesDataService;
            _synchronizationApi = synchronizationApi;
            _synchronizationRepository = synchronizationRepository;
        }
        public async Task<SynchronizationStatus> PostSynchronizationBatch(SynchronizationBatchEntity batch, CancellationToken cancellationToken)
        {
            var items = await _pimsAttributesDataService.ListSynchronizationItems(batch.Offset, batch.BatchTotal, batch.ProjectCode, batch.ModelCode);

            batch.Items = items.Select(i => SynchronizationItemEntity.For(batch.SyncId, batch.Id, batch.BatchNo, i)).ToList();

            var status = await _synchronizationApi.PostASynchronizationBatch(batch, cancellationToken);

            var synchronization = await _synchronizationRepository.GetSynchronization(batch.SyncId, cancellationToken) ?? throw new InvalidOperationException($"Synchronization Not found: {batch.SyncId}");

            var currentBatch = synchronization.Batches.First(b => b.Id == batch.Id);

            currentBatch.Items = batch.Items;

            await _synchronizationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return status;
        }
    }
}
