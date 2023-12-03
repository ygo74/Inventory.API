using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using Inventory.Common.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inventory.Configuration.Api.Application.Datacenters;
using MediatR;
using System.Threading;
using Inventory.Common.Application.Core;
using HotChocolate.Types.Pagination;
using Inventory.Common.Application.Graphql.Extensions;

namespace Inventory.Configuration.Api.Graphql.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class DatacenterQueries
    {
        /// <summary>
        /// Get Datacenter by Id
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="ctx"></param>
        /// <param name="id">Datacenter's id</param>
        /// <returns></returns>
        public async Task<Payload<DatacenterDto>> GetDatacenter([Service] IMediator mediator,
            CancellationToken cancellationToken, IResolverContext ctx,
            int id)
        {

            var request = new GetDatacenterByIdRequest
            {
                Id = id
            };

            var result = await mediator.Send(request, cancellationToken);
            return result;
        }

        /// <summary>
        /// Get Datacenter by Name
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="ctx"></param>
        /// <param name="name">Datacenter's name</param>
        /// <returns></returns>
        public async Task<Payload<DatacenterDto>> GetDatacenterByName([Service] IMediator mediator,
            CancellationToken cancellationToken, IResolverContext ctx,
            string name)
        {

            var request = new GetDatacenterByNameRequest
            {
                Name = name
            };

            var result = await mediator.Send(request, cancellationToken);
            return result;
        }


        [UsePaging(DefaultPageSize = 100)]
        public async Task<Connection<DatacenterDto>> GetDatacenters([Service] IMediator mediator,
                                                                    CancellationToken cancellationToken, IResolverContext ctx,
                                                                    GetDatacenterRequest request = null)
        {

            if (request == null) { request = new GetDatacenterRequest(); }
            request.Pagination = ctx.GetCursorPaggingRequest();

            var result = await mediator.Send(request, cancellationToken);
            var edges = result.Data.Select(e => new Edge<DatacenterDto>(e, e.Id.ToString())).ToList();

            var pageInfo = new ConnectionPageInfo(result.HasNext, result.HasPrevious, result.StartCursor, result.EndCursor);

            var connection = new Connection<DatacenterDto>(edges, pageInfo, result.TotalCount);
            return connection;
        }


        [UsePaging]
        [UseProjection]
        [UseSorting]
        [UseFiltering]
        public IQueryable<DatacenterDto> SearchDatacenters([Service] IDbContextFactory<ConfigurationDbContext> dbContextFactory, 
                                                       [Service] IMapper mapper,
                                                      IResolverContext ctx)
        {
            var dbContext = dbContextFactory.CreateDbContext();
            return mapper.ProjectTo<DatacenterDto>(dbContext.Datacenters, null);
        }

    }
}
