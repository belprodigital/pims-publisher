using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace PimsPublisher.Application.Adapters
{ 
    public interface IDbContextService
    {
        IDbContextTransaction GetCurrentTransaction() ;
        DbConnection GetDbConnection();
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
        Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);
    }
    
}