using Microsoft.EntityFrameworkCore;
using PimsPublisher.Application.Adapters;
using PimsPublisher.Domains;
using PimsPublisher.Domains.Entities;
using System.Data;
using System.Threading;

namespace PimsPublisher.Infrastructure.PublisherDb
{
    internal class SynchronizationRepository : ISynchronizationRepository
    {
        private readonly PublisherDbContext _publisherDbContext;
        public IUnitOfWork UnitOfWork => _publisherDbContext;

        public SynchronizationRepository(PublisherDbContext publisherDbContext)
        {
            _publisherDbContext = publisherDbContext;
        }

        public Guid Add(SynchronizationAggregate synchronization)
        {
            if (_publisherDbContext.Synchronizations.Any(s => s.Id == synchronization.Id)) throw new DbUpdateException($"Synchronization Id {synchronization.Id} is existed, Cannot insert duplicate Id");

            _publisherDbContext.Synchronizations.Add(synchronization);

            return synchronization.Id;
        }

        public async Task<SynchronizationAggregate?> GetSynchronization(Guid syncId, CancellationToken cancellationToken)
        {
            return await _publisherDbContext.Synchronizations.Include(s => s.Batches).FirstOrDefaultAsync(s => s.Id == syncId, cancellationToken);
        }


        public async Task<SynchronizationBatchEntity?> GetBatchEntity(Guid batchId, CancellationToken cancellationToken)
        {
            return await _publisherDbContext.SynchronizationBatches.Include(b => b.Items).FirstOrDefaultAsync(b => b.Id == batchId, cancellationToken);
        }

        public async Task<SynchronizationBatchEntity> AddListItemToBatch(Guid batchId, List<SynchronizationItemEntity> synchronizationItems)
        {
            var batchentity = await _publisherDbContext.SynchronizationBatches.Include(b => b.Items).FirstOrDefaultAsync(b => b.Id == batchId);

            if(batchentity == null) 
            {
                throw new InvalidOperationException("Not Found SynchronizationBatch to add items");
            }

            if (batchentity.Items.Any() == false)
            {
                await _publisherDbContext.SynchronizationItems.AddRangeAsync(synchronizationItems);
                await _publisherDbContext.SaveChangesAsync();
            }


            return batchentity;
        }
    }
}
