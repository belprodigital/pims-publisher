using PimsPublisher.Application.Integrations;

namespace PimsPublisher.Infrastructure.JobWorker
{
    internal interface IJobFactory
    {
        internal IJobHandler Create<TJMessage>() where TJMessage : IJobMessage ;
    }
}
