using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using PimsPublisher.Application.Adapters;
using PimsPublisher.Infrastructure.PimsDb;
using PimsPublisher.Infrastructure.PublisherDb;
using PimsPublisher.Infrastructure.K3dClients;
using PimsPublisher.Infrastructure.IntegrationMessages;
using PimsPublisher.Infrastructure.MediatorPipeline;

namespace PimsPublisher.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            string publisherConnectionName,
            IConfiguration Configuration)
        {
            services
               .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddMicrosoftIdentityWebApi(Configuration, "AzureAd")
               .EnableTokenAcquisitionToCallDownstreamApi()
               .AddDownstreamApi("K3dApim", Configuration.GetSection("K3DSynchronizationApi"))
               .AddInMemoryTokenCaches();
            services
                .AddScoped<ISynchronizationApi, SynchronizationApi>();

            services
                .AddPublisherDb(Configuration.GetConnectionString(publisherConnectionName))
                .AddIntegrationEventStorage()
                .TransactionBehavior()
                .AddScoped<IPimsAttributesDataService, PimsAttributesDataService>();

            return services;
        }
        
    }
}
