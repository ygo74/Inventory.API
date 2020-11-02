using Inventory.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.API.Infrastructure
{
    public interface IGraphQLService
    {
        Task<ILookup<int, Server>> GetServersByGroupAsync(IEnumerable<int> groupIds, CancellationToken token);
    }
}