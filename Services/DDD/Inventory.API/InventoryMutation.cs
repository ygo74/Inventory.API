using GraphQL;
using GraphQL.Types;
using Inventory.Domain.Models;
using Inventory.API.Types;
using Inventory.Domain;
using Inventory.API.Dto;
using System.ComponentModel.DataAnnotations;
using Inventory.API.Infrastructure;

namespace Inventory.API
{
    public class InventoryMutation : ObjectGraphType
    {
        public InventoryMutation(InventoryService inventoryService, GraphQLService graphQLService)
        {
            Field<ServerType, ServerDto>()
                .Name("createServer")
                .Description("Create New Server")
                .Argument<NonNullGraphType<ServerInputType>>("server", "server input")
                .ResolveAsync(async ctx =>
                {
                    var item = ctx.GetArgument<CreateServerDto>("server");

                    var subnetIp = System.Net.IPAddress.Parse(item.SubnetIp);
                    var server = await inventoryService.AddServerAsync(item.HostName, item.OsFamilly, item.Os, item.Environment, subnetIp);

                    return await graphQLService.GetOrFillServerData(server);
                });
        }
    }
}
