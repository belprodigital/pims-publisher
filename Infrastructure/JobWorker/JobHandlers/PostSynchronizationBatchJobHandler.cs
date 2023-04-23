using Hangfire.Logging;
using Microsoft.Extensions.Logging;
using PimsPublisher.Application.Adapters;
using PimsPublisher.Application.Integrations;
using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Infrastructure.JobWorker.JobHandlers
{
    public class PostSynchronizationBatchJobHandler
    {
        public string Name => nameof(PostSynchronizationBatchJobHandler);

        private readonly IPimsAttributesDataService _pimsAttributesDataService;
        private readonly ISynchronizationApi _synchronizationApi;
        private readonly ISynchronizationRepository _synchronizationRepository;
        private readonly ILogger<PostSynchronizationBatchJobHandler> _logger;

        public PostSynchronizationBatchJobHandler(
            IPimsAttributesDataService pimsAttributesDataService, 
            ISynchronizationApi synchronizationApi, 
            ISynchronizationRepository synchronizationRepository,
            ILoggerFactory loggerFactory)
        {
            _pimsAttributesDataService = pimsAttributesDataService ?? throw new ArgumentNullException(nameof(pimsAttributesDataService));
            _synchronizationApi = synchronizationApi ?? throw new ArgumentNullException(nameof(synchronizationApi));
            _synchronizationRepository = synchronizationRepository ?? throw new ArgumentNullException(nameof(synchronizationRepository));
            _logger = loggerFactory.CreateLogger<PostSynchronizationBatchJobHandler>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Run(SynchronizationBatchJobMessage jobMessage, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Run Hangifire Job for Message {0} {1}", jobMessage.GetType().Name, jobMessage.Id);

            if(jobMessage is SynchronizationBatchJobMessage)
            {
                SynchronizationBatchJobMessage syncBatchMsg = (SynchronizationBatchJobMessage)jobMessage;

                var synchronization = await _synchronizationRepository.GetSynchronization(syncBatchMsg.SyncId, cancellationToken) ?? throw new InvalidOperationException($"Synchronization Not found: {syncBatchMsg.SyncId}");
                
                var batch = synchronization.Batches.First(b => b.Id == syncBatchMsg.BatchId);

                var items = await _pimsAttributesDataService.ListSynchronizationItems(syncBatchMsg.Offset, syncBatchMsg.BatchTotal, syncBatchMsg.ProjectCode, syncBatchMsg.ModelCode);

                var Items = items.Select(i => SynchronizationItemEntity.For(syncBatchMsg.SyncId, syncBatchMsg.Id, syncBatchMsg.BatchNo, i)).ToList();

                batch.Items = Items;

                var status = await _synchronizationApi.PostASynchronizationBatch(batch, cancellationToken);

                await _synchronizationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            }
            else
            {
                throw new InvalidOperationException($"{nameof(jobMessage)} is NOT an implementation of type {typeof(SynchronizationBatchJobMessage).Name}");
            }
        }
    }
}
