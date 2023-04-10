using System;
using System.Threading;
using System.Threading.Tasks;

namespace PimsPublisher.Domains
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
