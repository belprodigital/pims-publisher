using Microsoft.Extensions.DependencyInjection;
using PimsPublisher.Infrastructure.JobWorker.JobHandlers;

namespace PimsPublisher.Infrastructure.JobWorker
{
    internal static class DependencyInjection
    {
        internal static IServiceCollection AddWorkerService(this IServiceCollection services)
             => services
                .AddTransient<IJobWorkerService, HangfireJobWorkerService>()
                .AddScoped<PostSynchronizationBatchJobHandler>();
    }
}
