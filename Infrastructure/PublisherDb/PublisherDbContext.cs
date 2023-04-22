using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PimsPublisher.Domains;
using PimsPublisher.Domains.Entities;
using PimsPublisher.Infrastructure.PublisherDb.Configurations;

namespace PimsPublisher.Infrastructure.PublisherDb
{

    public class PublisherDbContext : DbContext, IUnitOfWork, IDbContextService
    {
        public virtual DbSet<SynchronizationAggregate> Synchronizations { get; set; }
        public virtual DbSet<SynchronizationBatchEntity> SynchronizationBatches { get; set; }
        public virtual DbSet<SynchronizationItemEntity> SynchronizationItems { get; set; }

        private IDbContextTransaction _currentTransaction;
        private readonly IMediator _mediator;
        private readonly ILogger<PublisherDbContext> _logger;

        public PublisherDbContext(DbContextOptions<PublisherDbContext> options, IMediator mediator, ILoggerFactory loggerFactory):base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = loggerFactory?.CreateLogger<PublisherDbContext>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public PublisherDbContext(DbContextOptions<PublisherDbContext> options):base(options) { }

        public async Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this, _logger);

            int effectedRecords = await base.SaveChangesAsync(cancellationToken);

            return effectedRecords;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SynchronizationAggregateConfiguration());
            modelBuilder.ApplyConfiguration(new SynchronizationBatchEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SynchronizationItemEntityConfiguration());
        }


        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync(cancellationToken);
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public DbConnection GetDbConnection() => this.Database.GetDbConnection();
    }
}
