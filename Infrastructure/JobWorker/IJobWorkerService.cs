

using PimsPublisher.Application.Integrations;

namespace PimsPublisher.Infrastructure.JobWorker
{
    public interface IJobWorkerService
    {
        public Task CreateJobForMessage(IMessage messsage, Type messageType);
    }
}
