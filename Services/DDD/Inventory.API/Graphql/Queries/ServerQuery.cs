using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.API.Application.Dto;
using Inventory.API.Graphql.Types;
using Inventory.API.Infrastructure;
using System.Collections.Generic;

namespace Inventory.API.Graphql.Queries
{
    public class ServerQuery : ObjectGraphType
    {

        public ServerQuery(IDataLoaderContextAccessor accessor, GraphQLService graphQLService)
        {

            Field<ListGraphType<ServerType>, IReadOnlyList<ServerDto>>()
                    .Name("Servers")
                    .ResolveAsync(ctx =>
                    {
                        return graphQLService.GetAllServersAsync();
                    });

        }
    }
}
