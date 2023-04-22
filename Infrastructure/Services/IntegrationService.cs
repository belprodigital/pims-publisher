using System.Data.Common;
using Microsoft.Extensions.Logging;
using PimsPublisher.Application.Integrations;
using PimsPublisher.Application.Services;
using PimsPublisher.Infrastructure.IntegrationMessages;
using PimsPublisher.Infrastructure.JobWorker;
using PimsPublisher.Infrastructure.PublisherDb;

namespace PimsPublisher.Infrastructure.Services
{
    public class IntegrationService : IIntegrationService
    {
        private readonly ILogger<IntegrationService> _logger;
        private readonly IIntegrationMessageLogRepository _integrationMessageLogRepository;
        private readonly IDbContextService _dbContextService;
        private readonly IJobWorkerService _jobWorkerService;


        public IntegrationService(
            ILoggerFactory loggerFactory,
            PublisherDbContext? publisherDbContext,
            IJobWorkerService jobWorkerService,
            Func<DbConnection, ILoggerFactory, IIntegrationMessageLogRepository> integrationMessageLogRepository
        )
        {
            _logger = loggerFactory?.CreateLogger<IntegrationService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _dbContextService = publisherDbContext ?? throw new ArgumentNullException(nameof(publisherDbContext));
            _jobWorkerService = jobWorkerService ?? throw new ArgumentNullException( nameof(jobWorkerService));
            
            if( integrationMessageLogRepository == null) throw new ArgumentNullException(nameof(integrationMessageLogRepository));
            _integrationMessageLogRepository = integrationMessageLogRepository(_dbContextService.GetDbConnection(), loggerFactory);
        }

        public async Task PublishMessagesThroughMessageBusAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            var pendingLogEvents = await _integrationMessageLogRepository.RetrieveMessageLogsPendingToPublishAsync(transactionId, cancellationToken);

            foreach (var msg in pendingLogEvents)
            {
                _logger.LogInformation("----- Publishing integration message: {MessageId}-({@Message})", msg.Id, msg);

                try
                {
                    await _integrationMessageLogRepository.MarkMessageAsInProgressAsync(msg.Id, cancellationToken);
                    if(msg is IJobMessage)
                    {
                        await _jobWorkerService.CreateJobForMessage(msg, msg.GetType());
                    }
                    

                    await _integrationMessageLogRepository.MarkMessageAsPublishedAsync(msg.Id, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration message: {MessageId}", msg.Id);

                    await _integrationMessageLogRepository.MarkMessageAsFailedAsync(msg.Id, cancellationToken);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IMessage message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("----- Enqueuing integration message {MessageId} to repository ({@message})", message.Id, message);

            await _integrationMessageLogRepository.SaveMessageAsync(message, _dbContextService.GetCurrentTransaction(), cancellationToken);
        }
         
        public async Task<T> GetEventLogByEntityId<T>(Guid entityId, CancellationToken cancellationToken) where T : IMessage
        {
            return await _integrationMessageLogRepository.GetMessageLogByEntityId<T>(entityId, _dbContextService.GetCurrentTransaction(),cancellationToken);
        }
    }
}
