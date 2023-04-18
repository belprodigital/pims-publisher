using Microsoft.Extensions.DependencyInjection;
using PimsPublisher.Infrastructure.PublisherDb;

namespace PimsPublisher.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string publisherConnectionString)
            => services.AddPublisherDbContext(publisherConnectionString);
    }
}
