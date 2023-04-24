using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace PimsPublisher.Infrastructure
{
    public interface IDbContextService
    {
        IDbContextTransaction GetCurrentTransaction();
        DbConnection GetDbConnection();
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
        Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);
    }

}