using PimsPublisher.Application.Integrations;

namespace PimsPublisher.Infrastructure.JobWorker
{
    internal interface IJobHandler
    {
        internal string Name { get;}
        internal void Run(IJobMessage jobMessage);
    }
}
