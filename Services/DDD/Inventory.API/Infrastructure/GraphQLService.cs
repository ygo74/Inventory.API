using Inventory.Domain;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.API.Infrastructure
{
    public class GraphQLService : IGraphQLService
    {

        private readonly InventoryService _inventoryService;

        public GraphQLService(InventoryService inventoryService)
        {
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
        }


        #region "Servers"
        public async Task<ILookup<int, Server>> GetServersByGroupAsync(IEnumerable<int> groupIds, CancellationToken token)
        {
            var serverGroups = await _inventoryService.GetServersByGroupAsync(groupIds);
            return serverGroups.ToLookup(s => s.GroupId, s => s.Server);
        }

        #endregion

    }
}
