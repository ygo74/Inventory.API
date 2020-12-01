using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.API.Graphql.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Domain.Specifications;

namespace Inventory.API
{
    public class ServerQuery : ObjectGraphType
    {
        public ServerQuery(IDataLoaderContextAccessor accessor, IAsyncRepository<Server> serverRepository)
        {

            Name = "Server";

            Field<ListGraphType<ServerType>, IReadOnlyList<Server>>()
                .Name("Servers")
                .ResolveAsync(ctx =>
               {
                   return serverRepository.ListAsync(new ServerSpecification());
               });
        }
    }
}
