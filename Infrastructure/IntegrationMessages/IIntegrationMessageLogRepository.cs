using Microsoft.EntityFrameworkCore.Storage;
using PimsPublisher.Application.Integrations;

namespace PimsPublisher.Infrastructure.IntegrationMessages
{
    public interface IIntegrationMessageLogRepository
    {
        Task<IEnumerable<IMessage>> RetrieveMessageLogsPendingToPublishAsync(Guid transactionId, CancellationToken cancellationToken);
        Task MarkMessageAsInProgressAsync(Guid messageId, CancellationToken cancellationToken);
        Task MarkMessageAsPublishedAsync(Guid messageId, CancellationToken cancellationToken);
        Task MarkMessageAsFailedAsync(Guid messageId, CancellationToken cancellationToken);
        Task SaveMessageAsync(IMessage message, IDbContextTransaction dbContextTransaction, CancellationToken cancellationToken);
        Task<T> GetMessageLogByEntityId<T>(Guid entityId, IDbContextTransaction dbContextTransaction, CancellationToken cancellationToken) where T : IMessage;
    }
}
