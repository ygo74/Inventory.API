using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using Inventory.Domain.Base.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Graphql.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class DatacenterQueries
    {
        [UseFiltering]
        public Task<List<Datacenter>> GetDatacenters([Service] IAsyncRepository<Datacenter> _repository, IResolverContext ctx)
        {
            return _repository.ListAsync();
        }

        [UsePaging]
        [UseProjection]
        [UseSorting]
        [UseFiltering]
        public IQueryable<Datacenter> GetDatacenters2([Service] IDbContextFactory<ConfigurationDbContext> dbContextFactory, IResolverContext ctx)
        {
            var dbContext = dbContextFactory.CreateDbContext();
            return dbContext.Datacenters;
        }

    }
}
