using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.API.Infrastructure;
using Inventory.API.Models;
using Inventory.API.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API
{
    public class InventoryQuery : ObjectGraphType
    {
        public InventoryQuery(IDataLoaderContextAccessor accessor, InventoryContext dbContext)
        {
            Field<ListGraphType<ServerType>, List<Server>>()
                .Name("Servers")
                .ResolveAsync(ctx =>
               {
                   return dbContext.Servers.ToListAsync();
               });
        }
    }
}
