using GraphQL;
using GraphQL.Types;
using Inventory.API.Graphql.Types;
using Inventory.API.Graphql.InputTypes;
using Inventory.API.Application.Dto;
using Inventory.API.Application.Commands;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;

namespace Inventory.API.Graphql.Mutations
{
    public class ServerMutation : ObjectGraphType
    {
        public ServerMutation(IMediator mediator)
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
