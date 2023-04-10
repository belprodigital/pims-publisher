using PimsPublisher.Domains;
using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Application
{
    public interface ISynchronizationSessionRepository: IRepository<SynchronizationSession>
    {
        Guid Add(SynchronizationSession session);

    }
}