using PimsPublisher.Application.Integrations;

namespace PimsPublisher.Application.Services
{
    public interface IIntegrationService
    {
        Task PublishMessagesThroughMessageBusAsync(Guid transactionId, CancellationToken cancellationToken);
        Task AddAndSaveEventAsync(IMessage @event, CancellationToken cancellationToken);
        Task<T> GetEventLogByEntityId<T>(Guid entityId, CancellationToken cancellationToken) where T : IMessage;
    }
}
