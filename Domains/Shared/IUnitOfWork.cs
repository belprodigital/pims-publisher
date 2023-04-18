using System;
using System.Threading;
using System.Threading.Tasks;

namespace PimsPublisher.Domains
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
