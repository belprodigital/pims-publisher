using MediatR;
using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Domains.Events
{
    public class SynchronizationBatchCreatedEvent : IDomainEvent, INotification
    {
        public Guid Id { get; private set; }

        public Guid AggregationId { get; private set; }

        public SynchronizationBatchEntity BatchEntity { get; private set; }

        private SynchronizationBatchCreatedEvent(SynchronizationBatchEntity entity)
        {
            Id = Guid.NewGuid();
            AggregationId = entity.Id;
            BatchEntity = entity;
        }

        public static SynchronizationBatchCreatedEvent For(SynchronizationBatchEntity entity)
        {
           return new SynchronizationBatchCreatedEvent(entity);
        }
    }
}
