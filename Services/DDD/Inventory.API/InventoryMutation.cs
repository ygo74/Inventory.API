using GraphQL;
using GraphQL.Types;
using Inventory.Domain.Models;
using Inventory.API.Graphql.Types;
using Inventory.Domain;
using Inventory.API.Dto;
using System.ComponentModel.DataAnnotations;
using Inventory.API.Infrastructure;
using Inventory.API.Graphql.InputTypes;
using Inventory.API.Commands;
using MediatR;

namespace Inventory.API
{
    public class InventoryMutation : ObjectGraphType
    {
        public InventoryMutation(IMediator mediator)
        {
            Field<ServerType, ServerDto>()
                .Name("createServer")
                .Description("Create New Server")
                .Argument<NonNullGraphType<ServerInputType>>("server", "server input")
                .ResolveAsync(async ctx =>
                {
                    var item = ctx.GetArgument<CreateServerCommand>("server");

                    var serverDto = await mediator.Send(item);
                    return serverDto;
                });
        }
    }
}
