using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.API.Application.Dto;
using Inventory.API.Application.Servers;
using Inventory.API.Graphql.Filters;
using Inventory.API.Graphql.Types;
using Inventory.API.Infrastructure;
using Inventory.Domain.Filters;
using System.Collections.Generic;

namespace Inventory.API.Graphql.Queries
{
    public class ServerQuery : ObjectGraphType
    {

        public ServerQuery(IDataLoaderContextAccessor accessor, ServerService serverService)
        {

            Field<ListGraphType<ServerType>, IReadOnlyList<ServerDto>>()
                    .Name("Servers")
                    .Argument<ServerFilterType>("filter")
                    .ResolveAsync(ctx =>
                    {
                        var filter = ctx.GetArgument<ServerFilter>("filter");
                        filter.IsPagingEnabled = true;
                        return serverService.GetAllServersAsync(filter);
                    });
        }
    }
}
