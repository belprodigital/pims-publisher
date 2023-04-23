using Hangfire.Logging;
using Microsoft.Extensions.Logging;
using PimsPublisher.Application.Adapters;
using PimsPublisher.Application.Integrations;
using PimsPublisher.Domains.Entities;
using PimsPublisher.Infrastructure.K3dClients.DataContract;
using PimsPublisher.Infrastructure.PublisherDb;

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

            var items = await _pimsAttributesDataService.ListSynchronizationItems(jobMessage.Offset, jobMessage.BatchTotal, jobMessage.ProjectCode, jobMessage.ModelCode);
            var itemEntities = items.Select(i => SynchronizationItemEntity.For(jobMessage.SyncId, jobMessage.BatchId, jobMessage.BatchNo, i)).ToList();

            SynchronizationBatchEntity? batchEntity = await _synchronizationRepository.AddListItemToBatch(jobMessage.BatchId, itemEntities);

            if(batchEntity == null)
            {
                throw new InvalidOperationException($"Not Found Batch {jobMessage.BatchId} to execute function");
            }


            var status = await _synchronizationApi.PostASynchronizationBatch(batchEntity, cancellationToken);
        }
    }
}
