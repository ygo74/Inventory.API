using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.API.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Domain.Specifications;

namespace Inventory.API
{
    public class InventoryQuery : ObjectGraphType
    {
        public InventoryQuery(IDataLoaderContextAccessor accessor, IAsyncRepository<Server> serverRepository)
        {
            Field<ListGraphType<ServerType>, IReadOnlyList<Server>>()
                .Name("Servers")
                .ResolveAsync(ctx =>
               {
                   return serverRepository.ListAsync(new ServerSpecification());
               });
        }
    }
}
