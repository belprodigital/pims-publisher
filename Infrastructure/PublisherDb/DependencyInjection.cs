using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PimsPublisher.Application.Adapters;

namespace PimsPublisher.Infrastructure.PublisherDb
{
    internal static class DependencyInjection
    {
        internal static IServiceCollection AddPublisherDb(this IServiceCollection services, string PublisherDbConnectionString) 
            => services
            .AddDbContext<PublisherDbContext>(option => option.UseSqlServer(PublisherDbConnectionString))
            .AddScoped<ISynchronizationRepository, SynchronizationRepository>();
    }
}
