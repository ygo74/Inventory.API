using GraphQL.Types;
using Inventory.API.Application.Dto;
using Inventory.Domain.Models;
using System.Collections.Generic;

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
            //Field<ListGraphType<GroupType>, IEnumerable<Group>>()
            //        .Name("Groups")
            //        .Resolve(ctx =>
            //        {
            //            return ctx.Source.Groups;
            //        });

        }

    }
}
