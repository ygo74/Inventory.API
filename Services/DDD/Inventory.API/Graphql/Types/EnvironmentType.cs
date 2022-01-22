//using GraphQL.DataLoader;
//using GraphQL.Types;
//using Inventory.API.Application.Dto;
//using Inventory.API.Application.Servers;
//using Inventory.API.Infrastructure;
//using Inventory.Domain.Models;
//using Inventory.Domain.Repositories.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Inventory.API.Graphql.Types
//{
//    public class EnvironmentType : ObjectGraphType<Inventory.Domain.Models.Configuration.Environment>
//    {
//        public EnvironmentType(ServerService serverService, IDataLoaderContextAccessor accessor)
//        {
//            Field(e => e.Name);

//            //Servers
//            Field<ListGraphType<ServerType>, IEnumerable<ServerDto>>()
//                .Name("Servers")
//                .ResolveAsync(ctx =>
//                {
//                    var itemsloader = accessor.Context.GetOrAddCollectionBatchLoader<int, ServerDto>("GetServersByEnvironmentId", serverService.GetServersByEnvironmentAsync);
//                    return itemsloader.LoadAsync(ctx.Source.EnvironmentId);

//                });

//        }
//    }
//}
