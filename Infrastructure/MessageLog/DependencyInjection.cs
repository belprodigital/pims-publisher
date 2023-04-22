namespace PimsPublisher.Infrastructure.MessageLog
{
    public static class DependencyInjection
    {
        private static IServiceCollection AddIntegrationEventStorage(this IServiceCollection services) 
            =>  services                
                .AddTransient<Func<DbConnection, ILoggerFactory, IMessageLogService>>(sp => (DbConnection c, ILoggerFactory loggerFactory) => new IntegrationMessageLogService(c, loggerFactory))
                .AddTransient<IIntegrationMessageService, IntegrationMessageService>();
    }
}