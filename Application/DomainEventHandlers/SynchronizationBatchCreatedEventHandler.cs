using MediatR;
using Microsoft.Extensions.Logging;
using PimsPublisher.Application.Integrations;
using PimsPublisher.Application.Services;
using PimsPublisher.Domains.Events;


namespace PimsPublisher.Application.DomainEventHandlers
{
    public class SynchronizationBatchCreatedEventHandler : INotificationHandler<SynchronizationBatchCreatedEvent>
    {
        public readonly ILogger<SynchronizationBatchCreatedEventHandler> _logger;
        public readonly IIntegrationService _integrationService;

        public SynchronizationBatchCreatedEventHandler(ILoggerFactory loggerFactory, IIntegrationService integrationMessageService)
        {
            _logger = loggerFactory?.CreateLogger< SynchronizationBatchCreatedEventHandler>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _integrationService = integrationMessageService ?? throw new ArgumentNullException(nameof(integrationMessageService));
        }

        public async Task Handle(SynchronizationBatchCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _integrationService.AddAndSaveEventAsync(SynchronizationBatchJobMessage.For(notification.BatchEntity), cancellationToken);
        }
    }
}
