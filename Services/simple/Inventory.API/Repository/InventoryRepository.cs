using Inventory.API.Infrastructure;
using Inventory.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.API.Repository
{

    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryContext _inventoryContext;

        public InventoryRepository(InventoryContext inventoryContext)
        {
            _inventoryContext = inventoryContext != null ? inventoryContext : throw new ArgumentNullException(nameof(inventoryContext));
        }

        public async Task<IDictionary<int, Server>> GetServersByIdAsync(IEnumerable<int> serverIds, CancellationToken token)
        {
            return await _inventoryContext.Servers
                                          .Where(s => serverIds.Contains(s.ServerId))
                                          .ToDictionaryAsync(s => s.ServerId, cancellationToken: token);
        }

        /// <summary>
        /// Get Groups By Server
        /// </summary>
        /// <param name="serverIds"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ILookup<int, Group>> GetgroupsByServerAsync(IEnumerable<int> serverIds, CancellationToken token)
        {
            var serverGroups = await _inventoryContext.ServerGroups
                                          .Where(sg => serverIds.Contains(sg.ServerId))
                                          .Include(sg => sg.Group).ToListAsync();
//                                          .Select(sg => sg.Group).ToListAsync();

            return serverGroups.ToLookup(s => s.ServerId, s => s.Group );
                                          //.ToDictionaryAsync(s => $"{s.GroupId}}", cancellationToken: token);
        }

        public async Task<ILookup<int, Server>> GetServersByGroupAsync(IEnumerable<int> groupIds, CancellationToken token)
        {
            var serverGroups = await _inventoryContext.ServerGroups
                                          .Where(sg => groupIds.Contains(sg.GroupId))
                                          .Include(sg => sg.Server).ToListAsync();
            //                                          .Select(sg => sg.Group).ToListAsync();

            return serverGroups.ToLookup(s => s.GroupId, s => s.Server);
            //.ToDictionaryAsync(s => $"{s.GroupId}}", cancellationToken: token);
        }


    }
}
