using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PimsPublisher.Domains;

namespace PimsPublisher.Infrastructure.PublisherDb
{
    internal static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext ctx, ILogger logger = null)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity<Guid>>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());
            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents);


            logger?.LogTrace("domainEntities: { 0}", domainEntities.Count());
            logger?.LogTrace("Total domainEvents:{ 0}", domainEvents.Count());

            foreach (var domainEvent in domainEvents)
            {
                logger?.LogTrace("PUBLISH domainEvent: { domainEvent}", domainEvent.GetType().Name);
                await mediator.Publish(domainEvent);
            }
            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());
        }
    }
}
