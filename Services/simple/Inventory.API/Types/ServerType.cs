using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.Domain.Models;
using Inventory.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Types
{
    public class ServerType: ObjectGraphType<Server>
    {
        public ServerType(IInventoryRepository inventoryRepository, IDataLoaderContextAccessor accessor)
        {

            Field(s => s.ServerId);
            Field(s => s.Name);

            //Group
            Field<ListGraphType<GroupType>, IEnumerable<Group>>()
                .Name("Groups")
                .ResolveAsync(ctx =>
                {
                    var itemsloader = accessor.Context.GetOrAddCollectionBatchLoader<int, Group>("GetGroupsByServerId", inventoryRepository.GetgroupsByServerAsync);
                    return itemsloader.LoadAsync(ctx.Source.ServerId);

                });

        }
    }
}
