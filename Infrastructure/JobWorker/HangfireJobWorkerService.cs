using Hangfire;
using Microsoft.Extensions.Logging;
using PimsPublisher.Application.Integrations;
using PimsPublisher.Infrastructure.JobWorker.JobHandlers;

namespace PimsPublisher.Infrastructure.JobWorker
{
    internal class HangfireJobWorkerService : IJobWorkerService
    {
        private readonly ILogger<HangfireJobWorkerService> _logger;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public HangfireJobWorkerService(
            ILoggerFactory loggerFactory,
            IBackgroundJobClient backgroundJobClient)
        {
            _logger = loggerFactory?.CreateLogger<HangfireJobWorkerService>()?? throw new ArgumentNullException(nameof(loggerFactory));
            _backgroundJobClient = backgroundJobClient ?? throw new ArgumentNullException(nameof(backgroundJobClient));
        }
        public Task<string> CreateJobForMessage(IMessage messsage, Type messageType, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromResult(string.Empty);
            }

            if(messsage == null)
            {
                throw new ArgumentNullException(nameof(messsage));
            }

            if(messageType == typeof(SynchronizationBatchJobMessage))
            {
                var realMessage = messsage as SynchronizationBatchJobMessage;
                string jobId = _backgroundJobClient.Enqueue<PostSynchronizationBatchJobHandler>(job => job.Run(realMessage, CancellationToken.None));
                
                return Task.FromResult(jobId);
            }

            throw new InvalidOperationException($"Unknown HangireJob for message type '{messageType.FullName}'.");

        }
    }
}
