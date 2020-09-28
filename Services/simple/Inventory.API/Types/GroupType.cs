using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.API.Models;
using Inventory.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Types
{
    public class GroupType : ObjectGraphType<Group>
    {
        public GroupType(IInventoryRepository inventoryRepository, IDataLoaderContextAccessor accessor)
        {
            Field(g => g.GroupId);
            Field(g => g.Name);

            //Servers
            Field<ListGraphType<ServerType>, IEnumerable<Server>>()
                .Name("Servers")
                .ResolveAsync(ctx =>
                {
                    var itemsloader = accessor.Context.GetOrAddCollectionBatchLoader<int, Server>("GetServersByGroupId", inventoryRepository.GetServersByGroupAsync);
                    return itemsloader.LoadAsync(ctx.Source.GroupId);

                });


        }
    }
}
