using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.Infrastructure;
using Inventory.Domain.Models;
using Inventory.API.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Types
{
    public class ServerGroupType : ObjectGraphType<ServerGroup>
    {
        public ServerGroupType(IInventoryRepository inventoryRepository, IDataLoaderContextAccessor accessor)
        {
            //Server
            Field(s => s.ServerId);
            Field<ServerType, Server>()
                .Name("Server")
                .ResolveAsync(ctx =>
                {
                    var itemsloader = accessor.Context.GetOrAddBatchLoader<int, Server>("GetServerById", inventoryRepository.GetServersByIdAsync);
                    return itemsloader.LoadAsync(ctx.Source.ServerId);

                });

        }
    }
}
