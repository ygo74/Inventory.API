using AutoMapper;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Data.Sorting;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Inventory.Common.Application.Graphql.Extensions;
using Inventory.Configuration.Api.Application.Plugin;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using Inventory.Common.Domain.Repository;
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


# nullable enable

        [UsePaging(DefaultPageSize = 10)]
        public async Task<Connection<PluginDto>> GetPlugins([Service] IMediator mediator,
            CancellationToken cancellationToken, IResolverContext ctx, 
            string? code, string? name, string? version, DateTime? validFrom, DateTime? validTo, bool? includeDeprecated=false, bool? includeAllEntitites = false)
        {

            var request = new GetPluginRequest
            {
                Pagination = ctx.GetCursorPaggingRequest(),
                AllEntities = includeAllEntitites.Value,
                IncludeDeprecated = includeDeprecated.Value,
                Code = code,
                Name = name,
                Version = version,
                ValidFrom = validFrom, 
                ValidTo = validTo
            };

            var result = await mediator.Send(request, cancellationToken);
            var edges = result.Data.Select(e => new Edge<PluginDto>(e, e.Id.ToString())).ToList();

            var pageInfo = new ConnectionPageInfo(result.HasNext, result.HasPrevious, result.StartCursor , result.EndCursor);

            var connection = new Connection<PluginDto>(edges, pageInfo, result.TotalCount);
            return connection;
        }


        [UseOffsetPaging]
        public async Task<CollectionSegment<PluginDto>> GetPlugins2([Service] IMediator mediator,
            CancellationToken cancellationToken, IResolverContext ctx,
            string? code, string? name, string? version, bool? includeDeprecated = false, bool? includeAllEntitites = false)
        {

            var request = new GetPluginRequest2
            {
                Pagination = ctx.GetOffsetPagingRequest(),
                AllEntities = includeAllEntitites.Value,
                IncludeDeprecated = includeDeprecated.Value,
                Code = code,
            };

            var plugins = await mediator.Send(request, cancellationToken);

            var pageInfo = new CollectionSegmentInfo(false, false);

            var collectionSegment = new CollectionSegment<PluginDto>(
                plugins.Data,
                pageInfo,
                plugins.TotalCount);
                

            return collectionSegment;
        }

        

    }

# nullable disable

}
