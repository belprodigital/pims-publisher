using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace PimsPublisher.Infrastructure.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
        private readonly ExpenseDbContext _dbContext;
        private readonly IIntegrationMessageService _integrationEventService;

        public TransactionBehavior(ExpenseDbContext dbContext,
            IIntegrationMessageService integrationEventService,
            ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
            _integrationEventService = integrationEventService ?? throw new ArgumentException(nameof(integrationEventService));
            _logger = logger ?? throw new ArgumentException(nameof(ILogger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            
            var typeName = request.GetGenericTypeName();

            if(request is Application.Commons.IQuery<TResponse>)
            {
                return await next();
            }

            try
            {
                if (_dbContext.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    Guid transactionId;

                    using (var transaction = await _dbContext.BeginTransactionAsync(cancellationToken))
                    {
                        _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                        response = await next();

                        _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                        await _dbContext.CommitTransactionAsync(transaction, cancellationToken);

                        transactionId = transaction.TransactionId;
                    }

                    await _integrationEventService.PublishMessagesThroughMessageBusAsync(transactionId, cancellationToken);
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);

                throw;
            }
        }
    }
}