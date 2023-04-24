using System;

namespace PimsPublisher.Domains
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        Guid AggregationId { get; }
    }
}