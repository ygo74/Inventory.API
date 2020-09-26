using GraphQL;
using GraphQL.Types;
using Inventory.API.Infrastructure;
using Inventory.API.Models;
using Inventory.API.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API
{
    public class InventoryMutation : ObjectGraphType
    {
        public InventoryMutation(InventoryContext dbContext)
        {
            Field<ServerType, Server>()
                .Name("createServer")
                .Description("Create New Server")
                .Argument<NonNullGraphType<ServerInputType>>("server", "server input")
                .Resolve(ctx =>
                {
                    var item = ctx.GetArgument<Server>("server");
                    var addedItem = dbContext.Servers.Add(item);
                    dbContext.SaveChanges();
                    return addedItem.Entity;
                });
        }
    }
}
