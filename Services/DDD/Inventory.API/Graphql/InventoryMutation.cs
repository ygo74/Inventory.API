using GraphQL;
using GraphQL.Types;
using Inventory.Domain.Models;
using Inventory.API.Graphql.Types;
using Inventory.Domain;
using Inventory.API.Dto;
using FluentValidation;
using FluentValidation.Results;
using Inventory.API.Infrastructure;
using Inventory.API.Graphql.InputTypes;
using Inventory.API.Commands;
using MediatR;
using System;

namespace Inventory.API.Graphql
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

                    try
                    {

                        var item = ctx.GetArgument<CreateServerCommand>("server");
                        var newServer = await mediator.Send(item);
                        return newServer;
                    }
                    catch(ValidationException ve)
                    {
                        foreach (ValidationFailure error in ve.Errors)
                        {
                            ctx.Errors.Add(new ExecutionError($"{error.ErrorCode} : {error.ErrorMessage}"));
                        }
                        return null;
                    }
                    catch (Exception e)
                    {
                        ctx.Errors.Add(new ExecutionError(e.Message));
                        return null;
                    }
                });
        }
    }
}
