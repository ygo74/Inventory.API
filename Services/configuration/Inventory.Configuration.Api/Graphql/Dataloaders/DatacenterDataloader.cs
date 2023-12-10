using Ardalis.GuardClauses;
using Elastic.Apm.Api;
using GreenDonut;
using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Graphql.Dataloaders
{
    public class DatacenterDataloader
    {
    }

    public class PluginEndpointByDatacenterDataloader : GroupedDataLoader<int, DatacenterPluginsDto>
    {

        private readonly ILogger<PluginEndpointByDatacenterDataloader> _logger;
        private readonly IGenericQueryStore<Datacenter> _queryStore;

        public PluginEndpointByDatacenterDataloader(ILogger<PluginEndpointByDatacenterDataloader> logger,
                                                    IBatchScheduler batchScheduler,
                                                    IGenericQueryStore<Datacenter> queryStore,
                                                    DataLoaderOptions? options = null) : base(batchScheduler, options)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _queryStore = Guard.Against.Null(queryStore, nameof(queryStore));
        }

        protected override async Task<ILookup<int, DatacenterPluginsDto>> LoadGroupedBatchAsync(
            IReadOnlyList<int> keys, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Get plugins by datacenter ids {0}", keys);

            // Create filter
            var filter = ExpressionFilterFactory.Create<Datacenter>()
                            .ForMultipleDatacenter(keys);

            // Execute the query
            var result = await _queryStore.GetByCriteriaAsync<DatacenterPluginsDto>(criteria: filter,
                                                                              orderBy: q => q.OrderBy(e => e.Id),
                                                                              cancellationToken: cancellationToken);

            return result.ToLookup(e => e.DatacenterId);
        }
    }
}
