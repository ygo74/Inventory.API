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
        /// <summary>
        /// Test documentation on query
        /// </summary>
        /// <param name="dbContextFactory"></param>
        /// <param name="mapper"></param>
        /// <param name="pluginService"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        [UsePaging]
        [UseProjection]
        [UseSorting]
        [UseFiltering]
        public IQueryable<PluginDto> GetPlugins([Service] IDbContextFactory<ConfigurationDbContext> dbContextFactory,
                                                       [Service] IMapper mapper,
                                                       [Service] PluginService pluginService,
                                                      IResolverContext ctx)
        {
            var dbContext = dbContextFactory.CreateDbContext();
            //return mapper.ProjectTo<PluginDto>(dbContext.Plugins, null);
            //return dbContext.Plugins.Select(e => pluginService.GetPluginDto(e));
            return dbContext.Plugins.Convert(pluginService);

        }


    }

    public static class PluginConverters
    {
        public static IQueryable<PluginDto> Convert(this IQueryable<Plugin> plugins, PluginService pluginService)
        {
            return plugins.Select(e => pluginService.GetPluginDto(e));
        }
    }
}
