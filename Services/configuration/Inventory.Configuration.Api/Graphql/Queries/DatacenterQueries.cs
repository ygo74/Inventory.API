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
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inventory.Configuration.Api.Application.Datacenter;

namespace Inventory.Configuration.Api.Graphql.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class DatacenterQueries
    {
        [UseFiltering]
        public Task<List<DatacenterDto>> GetDatacenters([Service] IAsyncRepository<Datacenter> _repository, [Service] IMapper mapper, IResolverContext ctx)
        {
            return Task.FromResult(mapper.Map<List<DatacenterDto>>(_repository.ListAsync()));
        }

        [UsePaging]
        [UseProjection]
        [UseSorting]
        [UseFiltering]
        public IQueryable<DatacenterDto> GetDatacenters2([Service] IDbContextFactory<ConfigurationDbContext> dbContextFactory, 
                                                       [Service] IMapper mapper,
                                                      IResolverContext ctx)
        {
            var dbContext = dbContextFactory.CreateDbContext();
            return mapper.ProjectTo<DatacenterDto>(dbContext.Datacenters, null);
        }

    }
}
