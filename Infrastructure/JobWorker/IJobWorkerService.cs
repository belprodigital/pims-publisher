

using PimsPublisher.Application.Integrations;

namespace PimsPublisher.Infrastructure.JobWorker
{
    public interface IJobWorkerService
    {
        public Task<string> CreateJobForMessage(IMessage messsage, Type messageType, CancellationToken cancellationToken);
    }
}
