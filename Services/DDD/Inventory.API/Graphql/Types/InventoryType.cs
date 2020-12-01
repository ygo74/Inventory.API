using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.API.Dto;
using Inventory.API.Infrastructure;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Graphql.Types
{
    public class InventoryType : ObjectGraphType<InventoryDto>
    {
        public InventoryType()
        {
            //Servers
            Field<ListGraphType<ServerType>, IEnumerable<ServerDto>>()
                    .Name("Servers")
                    .Resolve(ctx =>
                    {
                        return ctx.Source.Servers;
                    });

            //Servers
            Field<ListGraphType<GroupType>, IEnumerable<Group>>()
                    .Name("Groups")
                    .Resolve(ctx =>
                    {
                        return ctx.Source.Groups;
                    });

        }

    }
}
