using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PimsPublisher.Application.Services;
using PimsPublisher.Infrastructure.Services;

namespace PimsPublisher.Infrastructure.IntegrationMessages
{
    public static class DependencyInjection
    {
        internal static IServiceCollection AddIntegrationEventStorage(this IServiceCollection services) 
            =>  services                
                .AddTransient<Func<DbConnection, ILoggerFactory, IIntegrationMessageLogRepository>>(sp => (DbConnection c, ILoggerFactory loggerFactory) => new IntegrationMessageLogRepository(c, loggerFactory))
                .AddTransient<IIntegrationService, IntegrationService>();
    }
}