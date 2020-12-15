using FluentValidation;
using FluentValidation.Results;
using GraphQL;
using GraphQL.Types;
using Inventory.API.Application.Commands;
using Inventory.API.Graphql.InputTypes;
using Inventory.API.Graphql.Types;
using Inventory.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Graphql.Mutations
{
    public class GroupMutation : ObjectGraphType
    {
        public GroupMutation(IMediator mediator)
        {
            Field<GroupType, Group>()
                .Name("createGroup")
                .Description("Create new group in the Groups database")
                .Argument<NonNullGraphType<GroupInputType>>("group", "Group Input")
                .ResolveAsync(async ctx =>
                {
                    try
                    {
                        var item = ctx.GetArgument<CreateGroupCommand>("group");
                        var newGroup = await mediator.Send(item);
                        return newGroup;
                    }
                    catch (ValidationException ve)
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
