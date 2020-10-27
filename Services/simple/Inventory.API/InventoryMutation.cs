using GraphQL;
using GraphQL.Types;
using Inventory.Infrastructure;
using Inventory.Domain.Models;
using Inventory.API.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.API.Repository;

namespace Inventory.API
{
    public class InventoryMutation : ObjectGraphType
    {
        public InventoryMutation(InventoryDbContext dbContext, InventoryRepository inventoryRepository)
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

            Field<GroupType, Group>()
                .Name("CreateGroup")
                .Description("Create New Group")
                .Argument<NonNullGraphType<GroupInputType>>("group", "group definition")
                .Resolve(ctx =>
                {
                    var item = ctx.GetArgument<Group>("group");
                    var newGroup = inventoryRepository.CreateGroup(item.Name, item.Parent != null ? item.Parent.Name : null);
                    inventoryRepository.SaveChangesAsync().Wait();
                    return newGroup;

                });


        }
    }
}
