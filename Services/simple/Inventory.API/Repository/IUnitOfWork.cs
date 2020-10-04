using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.API.Repository
{
    public interface IUnitOfWork
    {        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
