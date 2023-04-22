using PimsPublisher.Application.Integrations;

namespace PimsPublisher.Infrastructure.JobWorker
{
    internal class SendSynchroniszationBatchJobHandler : IJobHandler
    {
        string IJobHandler.Name => throw new NotImplementedException();

        void IJobHandler.Run(IJobMessage jobMessage)
        {
            throw new NotImplementedException();
        }
    }
}
