using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PimsPublisher.Application.Integrations;

namespace PimsPublisher.Infrastructure.IntegrationMessages
{
    public class IntegrationMessageLogRepository : IIntegrationMessageLogRepository, IDisposable
    {
        private readonly IntegrationMessageLogContext _integrationMessageLogContext;
        private readonly List<Type> _messageTypes;
        private volatile bool disposedValue;

        public ILoggerFactory LoggerFactory { get; }

        public IntegrationMessageLogRepository(DbConnection dbConnection, ILoggerFactory loggerFactory)
        {
            DbConnection _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _integrationMessageLogContext = new IntegrationMessageLogContext(
                new DbContextOptionsBuilder<IntegrationMessageLogContext>()
                    .UseSqlServer(_dbConnection)
                    .Options);

            var assemblyContainIntergrationEvent = Assembly.GetAssembly(typeof(IMessage));

            _messageTypes = Assembly.Load(assemblyContainIntergrationEvent.FullName)
                .GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IMessage)))
                .ToList();
        }

        public Task SaveMessageAsync(IMessage message, IDbContextTransaction dbContextTransaction, CancellationToken cancellationToken)
        {
            if (dbContextTransaction == null) throw new ArgumentNullException(nameof(dbContextTransaction));

            var messageLogEntry = new IntegrationMessageLogEntry(message, dbContextTransaction.TransactionId);

            _integrationMessageLogContext.Database.UseTransaction(dbContextTransaction.GetDbTransaction());
            _integrationMessageLogContext.IntegrationMessageLogs.Add(messageLogEntry);

            return _integrationMessageLogContext.SaveChangesAsync(cancellationToken);
        }

        public Task MarkMessageAsPublishedAsync(Guid messageId, CancellationToken cancellationToken)
        {
            return UpdateMessageStatus(messageId, MessageStates.Published, cancellationToken);
        }

        public Task MarkMessageAsInProgressAsync(Guid messageId, CancellationToken cancellationToken)
        {
            return UpdateMessageStatus(messageId, MessageStates.InProgress, cancellationToken);
        }

        public Task MarkMessageAsFailedAsync(Guid messageId, CancellationToken cancellationToken)
        {
            return UpdateMessageStatus(messageId, MessageStates.PublishedFailed, cancellationToken);
        }

        private Task UpdateMessageStatus(Guid messageId, MessageStates status, CancellationToken cancellationToken)
        {
            var messageLogEntry = _integrationMessageLogContext.IntegrationMessageLogs.Single(ie => ie.MessageId == messageId);
            messageLogEntry.State = status;

            if (status == MessageStates.InProgress)
                messageLogEntry.TimesSent++;

            _integrationMessageLogContext.IntegrationMessageLogs.Update(messageLogEntry);

            return _integrationMessageLogContext.SaveChangesAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _integrationMessageLogContext?.Dispose();
                }


                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<IMessage>> RetrieveMessageLogsPendingToPublishAsync(Guid transactionId, CancellationToken cancellationToken)
        {
              var tid = transactionId.ToString();

            var result = await _integrationMessageLogContext.IntegrationMessageLogs
                .Where(e => e.TransactionId == tid && e.State == MessageStates.NotPublished).ToListAsync(cancellationToken);

            if (result != null && result.Any())
            {
                var MessageEntries = result.OrderBy(o => o.OccurredTime)
                    .Select(e => e.DeserializePayload(_messageTypes.Find(t => t.FullName == e.MessageTypeName)));
                
                return MessageEntries.Select(x =>x.IntegrationEvent);
            }

            return new List<IMessage>();
        }

        public async Task<T> GetMessageLogByEntityId<T>(Guid entityId, IDbContextTransaction dbContextTransaction, CancellationToken cancellationToken) where T : IMessage
        {
            //The query happen between transaction begin & commit need to provide IDbContextTransaction
            var dbTransaction = dbContextTransaction?.GetDbTransaction();
            if(dbTransaction != null)
                _integrationMessageLogContext.Database.UseTransaction(dbTransaction);
            
            var result = await _integrationMessageLogContext.IntegrationMessageLogs
                .Where(x => x.EntityId == entityId && x.MessageTypeShortName == typeof(T).ShortDisplayName())
                .OrderByDescending(x => x.OccurredTime)
                .FirstOrDefaultAsync(cancellationToken);

            var messagePayload = result.DeserializePayload(typeof(T));

            return (T)messagePayload.IntegrationEvent;
        }
    }
}
