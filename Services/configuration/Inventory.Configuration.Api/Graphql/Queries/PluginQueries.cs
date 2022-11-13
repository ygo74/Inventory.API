using AutoMapper;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Inventory.Configuration.Api.Application.Plugin;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using Inventory.Domain.Base.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Graphql.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class PluginQueries
    {

        [UsePaging]
        [UseProjection]
        [UseSorting]
        [UseFiltering]
        public IQueryable<PluginDto> GetPlugins([Service] IDbContextFactory<ConfigurationDbContext> dbContextFactory,
                                                       [Service] IMapper mapper,
                                                      IResolverContext ctx)
        {
            var dbContext = dbContextFactory.CreateDbContext();
            return mapper.ProjectTo<PluginDto>(dbContext.Plugins, null);
        }

    }
}
