using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.API.Application.Dto;
using Inventory.API.Application.Servers;
using Inventory.API.Infrastructure;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Graphql.Types
{
    public class ApplicationType : ObjectGraphType<Inventory.Domain.Models.Application>
    {
        public ApplicationType(ServerService serverService, IDataLoaderContextAccessor accessor)
        {
            Field(e => e.Name);
            Field(e => e.Code);

            //Servers
            Field<ListGraphType<ServerType>, IEnumerable<ServerDto>>()
                .Name("Servers")
                .ResolveAsync(ctx =>
                {
                    //var itemsloader = accessor.Context.GetOrAddCollectionBatchLoader<int, ServerDto>("GetServersByApplicationId", serverService.GetServersByApplicationAsync);
                    //return itemsloader.LoadAsync(ctx.Source.ApplicationId);
                    return null;

                });

        }
    }
}
