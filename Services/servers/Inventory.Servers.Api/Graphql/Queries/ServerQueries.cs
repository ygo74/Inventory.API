using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Inventory.Domain.Base.Repository;
using Inventory.Servers.Domain.Models;
using Inventory.Servers.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Servers.Api.Graphql.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class ServerQueries
    {
        /// <summary>
        /// Return server current date-time
        /// </summary>
        /// <returns>DateTime current date time</returns>
        //public DateTime GetServerDateTime() => DateTime.Now;

        public string GetStatus() => "OK";

        [UseFiltering]
        public Task<List<Server>> GetServers([Service]IAsyncRepository<Server> _repository, IResolverContext ctx)
        {
            return _repository.ListAsync();
        }

        [UsePaging]
        [UseSorting]
        [UseFiltering]
        public IQueryable<Server> GetServers2([Service] IDbContextFactory<ServerDbContext> dbContextFactory, IResolverContext ctx)
        {
            var dbContext = dbContextFactory.CreateDbContext();
            return dbContext.Servers;
        }

    }
}
