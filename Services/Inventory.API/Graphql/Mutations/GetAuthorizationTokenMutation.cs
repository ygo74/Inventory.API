using FluentValidation;
using FluentValidation.Results;
using GraphQL;
using GraphQL.Types;
using Inventory.API.Application.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Graphql.Mutations
{
    public class GetAuthorizationTokenMutation : ObjectGraphType
    {
        public GetAuthorizationTokenMutation(IMediator mediator)
        {
            Field<StringGraphType>()
                .Name("GetAuthorizationToken")
                .Description("Get Authorization Token for service")
                .ResolveAsync(async ctx =>
                {
                    try
                    {
                        var item = new GetAuthorizationTokenCommand();
                        var token = await mediator.Send(item);
                        return token;
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
