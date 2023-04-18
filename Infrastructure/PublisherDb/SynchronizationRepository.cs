using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using PimsPublisher.Application.Adapters;
using PimsPublisher.Domains;
using PimsPublisher.Domains.Entities;
using System.Data;

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
            return await _publisherDbContext.Synchronizations.FirstOrDefaultAsync(s => s.Id == syncId, cancellationToken);
        }
    }
}
