using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Domain.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
     //   Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
