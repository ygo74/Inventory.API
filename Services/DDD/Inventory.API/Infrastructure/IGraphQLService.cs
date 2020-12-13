using Inventory.API.Application.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.API.Infrastructure
{
    public interface IGraphQLService
    {
        Task<ILookup<int, ServerDto>> GetServersByGroupAsync(IEnumerable<int> groupIds, CancellationToken token);
        Task<InventoryDto> GetInventoryForGroupAsync(string groupName, string environment);
    }
}