using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PimsPublisher.Application.Adapters;
using PimsPublisher.Infrastructure.PimsDb;
using PimsPublisher.Infrastructure.PublisherDb;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
                .AddPublisherDbContext(Configuration.GetConnectionString(publisherConnectionName))
                .AddScoped<IPimsAttributesDataService, PimsAttributesDataService>()
                .AddScoped<ISynchronizationRepository, SynchronizationRepository>()
                .AddScoped<ISynchronizationApi, SynchronizationApi>();

            return services;

        }
        
    }
}
