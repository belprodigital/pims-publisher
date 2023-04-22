using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Expenses.Application.Messages;
using Expenses.Application.Interfaces;
using Expenses.Infrastructure.ExpenseDb;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace PimsPublisher.Infrastructure.Services
{
    public class IntegrationMessageService : IIntegrationMessageService
    {
        private readonly ILogger<IntegrationMessageService> _logger;
        private readonly IMessageLogService _messageLogService;
        private readonly IBus _bus;
        private readonly IDbContextService _dbContextService;

        public IntegrationMessageService(
            ILoggerFactory loggerFactory,
            IBus bus,
            PublisherDbContext publisherDbContext,
            Func<DbConnection, ILoggerFactory, IMessageLogService> integrationEventLogServiceFactory
        )
        {
            _logger = loggerFactory !=null ? loggerFactory.CreateLogger<IntegrationMessageService>() : throw new ArgumentNullException(nameof(loggerFactory));
            _dbContextService = publisherDbContext ?? throw new ArgumentNullException(nameof(publisherDbContext));
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            
            if( integrationEventLogServiceFactory == null) throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _messageLogService = integrationEventLogServiceFactory(_dbContextService.GetDbConnection(), loggerFactory);
        }

        public async Task PublishMessagesThroughMessageBusAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            var pendingLogEvents = await _messageLogService.RetrieveMessageLogsPendingToPublishAsync(transactionId, cancellationToken);

            foreach (var msg in pendingLogEvents)
            {
                _logger.LogInformation("----- Publishing integration message: {MessageId}-({@Message})", msg.Id, msg);

                try
                {
                    await _messageLogService.MarkMessageAsInProgressAsync(msg.Id, cancellationToken);
                    if(msg is ICommandMessage)
                    {
                        await _bus.Send(msg, msg.GetType());
                    }
                    else
                    {
                        await _bus.Publish(msg, msg.GetType());
                    }

                    await _messageLogService.MarkMessageAsPublishedAsync(msg.Id, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration message: {MessageId}", msg.Id);

                    await _messageLogService.MarkMessageAsFailedAsync(msg.Id, cancellationToken);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IMessage message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("----- Enqueuing integration message {MessageId} to repository ({@message})", message.Id, message);

            await _messageLogService.SaveMessageAsync(message, _dbContextService.GetCurrentTransaction(), cancellationToken);
        }
         
        public async Task<T> GetEventLogByEntityId<T>(Guid entityId, CancellationToken cancellationToken) where T : IMessage
        {
            return await _messageLogService.GetMessageLogByEntityId<T>(entityId, _dbContextService.GetCurrentTransaction(),cancellationToken);
        }
    }
}
