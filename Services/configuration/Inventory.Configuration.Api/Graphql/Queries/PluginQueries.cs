using AutoMapper;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Data.Sorting;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Inventory.Api.Base.Graphql.Extensions;
using Inventory.Configuration.Api.Application.Plugin;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using Inventory.Domain.Base.Repository;
using MediatR;
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
        /// Full free search on plugins
        /// Use offsetpagination for direct database access as cursor based pagination is not really implemented
        /// https://github.com/ChilliCream/hotchocolate/issues/5563 or usepaging to have the same contract even if cursor is indice for offset query
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="pluginService"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        [UsePaging]
        [UseProjection]
        [UseSorting]
        [UseFiltering]
        public IQueryable<Plugin> SearchPlugins(ConfigurationDbContext dbContext,
                                                       [Service] IMapper mapper,
                                                       [Service] PluginService pluginService,
                                                      IResolverContext ctx)
        {
            //https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Data/src/Data/Sorting/Context/SortingContextResolverContextExtensions.cs
            var argument = ctx.Selection.Field.Arguments["order"];
            var sorting = ctx.ArgumentLiteral<IValueNode>("order");
            var sortingInput = (ISortInputType)(((HotChocolate.Types.ListType)argument.Type).ElementType().NamedType());
            foreach (var fieldValue in ((ObjectValueNode)sorting).Fields)
            {
                if (sortingInput.Fields.TryGetField(fieldValue.Name.Value, out var field))
                {
                    System.Diagnostics.Debug.Write(field.Name);
                    System.Diagnostics.Debug.Write(fieldValue.Name.Value);
                    System.Diagnostics.Debug.Write(fieldValue.Value.Value);
                }
            }
            return dbContext.Plugins;
        }


        [UsePaging()]
        public async Task<Connection<PluginDto>> GetPlugins([Service] IMediator mediator,
            CancellationToken cancellationToken, IResolverContext ctx, string? code, bool includeDeprecated=false, bool includeAllEntitites = false)
        {

            var paginationArguments = ctx.GetPaggingArguments();
            var request = new GetPluginRequest
            {
                After = paginationArguments.After,
                Before = paginationArguments.Before,
                First = paginationArguments.First,
                Last = paginationArguments.Last,
                AllEntities = includeAllEntitites,
                IncludeDeprecated = includeDeprecated,
                Code = code,                
            };

            var plugins = await mediator.Send(request, cancellationToken);
            var edges = plugins.Result.Select(e => new Edge<PluginDto>(e, e.Id.ToString())).ToList();

            var firstCursor = plugins.Result.Count > 0 ? plugins.Result[0].Id.ToString() : "";
            var lastCursor = plugins.Result.Count > 0 ? plugins.Result[plugins.Result.Count -1].Id.ToString() : "";

            var pageInfo = new ConnectionPageInfo(plugins.HasNext, plugins.HasPrevious, firstCursor , lastCursor);

            var connection = new Connection<PluginDto>(edges, pageInfo, plugins.Count);
            return connection;
        }


        [UsePaging(typeof(PluginDto), IncludeTotalCount = true, DefaultPageSize = 10)]
        public IQueryable<PluginDto> GetValidPlugins(ConfigurationDbContext dbContext,
                                                       [Service] IMapper mapper,
                                                       [Service] PluginService pluginService,
                                                      IResolverContext ctx)
        {
            //return mapper.ProjectTo<PluginDto>(dbContext.Plugins, null);
            //return dbContext.Plugins.Select(e => pluginService.GetPluginDto(e));
            var validPlugin = new ValidPluginFilter();
            return dbContext.Plugins
                        .AsNoTracking()
                        .Where(validPlugin.Predicate)
                        .OrderBy(e => e.Code)
                        .Select(e => new PluginDto
                        {
                            Code = e.Code
                        });
            

        }

        

    }

    [ExtendObjectType(typeof(object), IgnoreProperties = new[] { "Last", "last" })]
    public class PluginsExtension
    {

    }
}
