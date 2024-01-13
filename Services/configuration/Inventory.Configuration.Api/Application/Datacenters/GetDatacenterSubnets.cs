using Ardalis.GuardClauses;
using Inventory.Common.Application.Plugins;
using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Domain.Models;
using Inventory.Networks.Domain.Models;
using Inventory.Plugins.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenters
{
    public class GetDatacenterSubnetsRequest : IRequest<List<Subnet>>
    {
        public string DatacenterCode { get; set; }
    }

    public class GetDatacenterSubnetsHandler : IRequestHandler<GetDatacenterSubnetsRequest, List<Subnet>>
    {

        private readonly ILogger<GetDatacenterSubnetsHandler> _logger;
        private readonly IGenericQueryStore<Datacenter> _queryStore;
        private readonly IPluginResolver _pluginResolver;

        public GetDatacenterSubnetsHandler(ILogger<GetDatacenterSubnetsHandler> logger, IGenericQueryStore<Datacenter> queryStore, IPluginResolver pluginResolver)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _queryStore = Guard.Against.Null(queryStore, nameof(queryStore));
            _pluginResolver = Guard.Against.Null(pluginResolver, nameof(pluginResolver));
        }

        public async Task<List<Subnet>> Handle(GetDatacenterSubnetsRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start getting subnets for datacenter with code '{0}'", request.DatacenterCode);

            // Read the datacenter plugins
            var filter = ExpressionFilterFactory.Create<Datacenter>().WithCode(request.DatacenterCode);
            var datacenterPLugins = await _queryStore.GetByCriteriaWithManySelectAsync<DatacenterPluginsDto>(
                criteria: filter,
                Projection: DatacenterPluginsDto.Projection
            );

            var datacenter = _queryStore.GetQuery(
                criteria: filter
                //includes: new Expression<Func<Datacenter, object>>[] 
                //                { e => e.Plugins.Select(c => c.Credential), e => e.Plugins.Select(p => p.Plugin)}
            );

            var result = await datacenter.Include(e => e.Plugins).ThenInclude(p => p.Credential)
                                   .Include(e => e.Plugins).ThenInclude(p => p.Plugin)
                                   .FirstOrDefaultAsync();
            

            if (datacenterPLugins == null || datacenterPLugins.Count() == 0) 
            {
                _logger.LogWarning("Don't find plugins for datacenter with code '{0}'", request.DatacenterCode);
                return default(List<Subnet>);
            }

            // Get the subnet provider
            List<Subnet> allSubnets = new List<Subnet>();
            foreach (var pluginEndpoint in datacenterPLugins)
            {
                var subnetProvider = _pluginResolver.GetService<ISubnetProvider>(pluginEndpoint.PluginCode);
                if (subnetProvider == null) continue;

                subnetProvider.InitCredential(
                    userName: pluginEndpoint.CredentialName,
                    description: pluginEndpoint.CredentialDescription,
                    propertyBag: pluginEndpoint.CredentialPropertyBag
                );

                allSubnets.AddRange(await subnetProvider.ListAllAsync());

            }

            _logger.LogInformation("End getting subnets for datacenter with code '{0}'", request.DatacenterCode);
            return allSubnets;
        }
    }

}
