using Ardalis.GuardClauses;
using HotChocolate;
using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Inventory.Configuration.Infrastructure;

namespace Inventory.Configuration.Api.Application.Datacenters
{
    public class GetDatacenterPluginEnpoint
    {
    }

    public class GetPluginsByDatacenterIdRequest : IRequest<List<DatacenterPluginsDto>>
    {
        public IEnumerable<int> DatacenterIds { get; set; }
        public Optional<string?> p { get; set; }
    }

    public class GetPluginsByDatacenterHandlers : IRequestHandler<GetPluginsByDatacenterIdRequest, List<DatacenterPluginsDto>>
    {
        private readonly ILogger<GetPluginsByDatacenterHandlers> _logger;
        private readonly IGenericQueryStore<Datacenter> _queryStore;
        private readonly IDbContextFactory<ConfigurationDbContext> _dbContextFactory;

        public GetPluginsByDatacenterHandlers(ILogger<GetPluginsByDatacenterHandlers> logger, IGenericQueryStore<Datacenter> queryStore,
            IDbContextFactory<ConfigurationDbContext> dbContextFactory)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _queryStore = Guard.Against.Null(queryStore, nameof(queryStore));
            _dbContextFactory = Guard.Against.Null(dbContextFactory, nameof(dbContextFactory));
        }

        public async Task<List<DatacenterPluginsDto>> Handle(GetPluginsByDatacenterIdRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Get plugins by datacenter ids {0}", request.DatacenterIds);

            // Create filter
            var filter = ExpressionFilterFactory.Create<Datacenter>()
                            .ForMultipleDatacenter(request.DatacenterIds);

            // Execute the query
            var result = await _queryStore.GetByCriteriaAsync<DatacenterPluginsDto>(criteria: filter,
                                                                              ManyProjection: DatacenterPluginsDto.Projection,   
                                                                              orderBy: q => q.OrderBy(e => e.Id),
                                                                              cancellationToken: cancellationToken);
            using var dbContext = _dbContextFactory.CreateDbContext();

            //var result = await dbContext.Datacenters
            //    .Where(e => request.DatacenterIds.Contains(e.Id))
            //    .SelectMany(p => p.Plugins
            //        .Select(e => new DatacenterPluginsDto
            //        {
            //            Id = e.Id,
            //            DatacenterId = p.Id,
            //            CredentialName = e.Credential.Name,
            //            CredentialDescription = e.Credential.Description,
            //            CredentialPropertyBag = e.Credential.PropertyBag,
            //            PluginName = e.Plugin.Name,
            //        }))
            //    .ToListAsync(cancellationToken);

            //var result = await dbContext.Datacenters
            //    .Where(e => request.DatacenterIds.Contains(e.Id))
            //    .SelectMany(DatacenterPluginsDto.Projection)
            //    .ToListAsync(cancellationToken);

            return result.ToList();
        }
    }


}
