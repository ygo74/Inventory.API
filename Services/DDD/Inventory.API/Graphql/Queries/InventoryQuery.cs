using GraphQL.DataLoader;
using GraphQL.Types;
using GraphQL.Server.Authorization.AspNetCore;

using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.API.Graphql.Types;
using System;
using GraphQL;
using Inventory.API.Infrastructure;
using Inventory.API.Application.Dto;

namespace Inventory.API.Graphql.Queries
{
    public class InventoryQuery : ObjectGraphType
    {
        public InventoryQuery(IAsyncRepository<Server> serverRepository, IGroupRepository groupRepository, GraphQLService graphQLService)
        {

            Field<InventoryType, InventoryDto>()
                .Name("Inventory")
                .AuthorizeWith("ansible")
                .Argument<NonNullGraphType<StringGraphType>>("GroupName")
                .Argument<NonNullGraphType<StringGraphType>>("Environment")
                .ResolveAsync(ctx =>
                {
                    var groupName = ctx.GetArgument<String>("GroupName");
                    var env = ctx.GetArgument<String>("Environment");
                    ctx.UserContext.Add("environment", env);

                    return graphQLService.GetInventoryForGroupAsync(groupName,env);

                });


        }
    }
}
