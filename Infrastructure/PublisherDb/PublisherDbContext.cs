using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PimsPublisher.Domains;
using PimsPublisher.Domains.Entities;
using PimsPublisher.Infrastructure.PublisherDb.Configurations;

namespace PimsPublisher.Infrastructure.PublisherDb
{

    public class PublisherDbContext : DbContext, IUnitOfWork
    {
        public virtual DbSet<SynchronizationAggregate> Synchronizations { get; set; }
        public virtual DbSet<SynchronizationBatchEntity> SynchronizationBatches { get; set; }
        public virtual DbSet<SynchronizationItemEntity> SynchronizationItems { get; set; }

        public readonly IMediator _mediator;
        public readonly ILogger<PublisherDbContext> _logger;

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
    }
}
