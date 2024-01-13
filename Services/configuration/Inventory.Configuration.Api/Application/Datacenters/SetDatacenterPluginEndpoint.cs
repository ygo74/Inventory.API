using Ardalis.GuardClauses;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Errors;
using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenters
{
    public class SetDatacenterPluginEndpointRequest : IRequest<Payload<IEnumerable<DatacenterPluginsDto>>>
    {

        public enum PluginEndpointAction
        {
            Add,
            Remove
        }

        public string DatacenterCode { get; set; }
        public string CredentialName { get; set; }
        public string PluginCode { get; set; }
        public PluginEndpointAction Action { get; set; }
    }

    public class SetDatacenterPluginEndpointHandler : IRequestHandler<SetDatacenterPluginEndpointRequest, Payload<IEnumerable<DatacenterPluginsDto>>>
    {

        private readonly IAsyncRepository<Datacenter> _datacenterRepository;
        private readonly IAsyncRepository<Credential> _credentialRepository;
        private readonly IAsyncRepository<Plugin> _pluginRepository;
        private readonly ILogger<SetDatacenterPluginEndpointHandler> _logger;


        public SetDatacenterPluginEndpointHandler(IAsyncRepository<Datacenter> datacenterRepository,
                                                  IAsyncRepository<Credential> credentialRepository,
                                                  IAsyncRepository<Plugin> pluginRepository,
                                                  ILogger<SetDatacenterPluginEndpointHandler> logger)
        {
            _datacenterRepository = Guard.Against.Null(datacenterRepository, nameof(datacenterRepository));
            _credentialRepository = Guard.Against.Null(credentialRepository, nameof(credentialRepository));
            _pluginRepository = Guard.Against.Null(pluginRepository, nameof(pluginRepository));
            _logger = Guard.Against.Null(logger, nameof(logger));
        }

        public async Task<Payload<IEnumerable<DatacenterPluginsDto>>> Handle(SetDatacenterPluginEndpointRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Manage Plugin {0} endpoint with credential {1} for datacenter {2}", request.PluginCode, request.CredentialName, request.DatacenterCode);

            // Retrieve DataCenter
            var filterDatacenter = ExpressionFilterFactory.Create<Datacenter>().WithCode(request.DatacenterCode);
            var datacenter = await _datacenterRepository.FirstOrDefaultAsync(filterDatacenter, cancellationToken: cancellationToken);
            if (null == datacenter)
            {
                var errorMessage = $"Don't find Datacenter with Code '{request.DatacenterCode}'";
                _logger.LogWarning(errorMessage);
                return Payload<IEnumerable<DatacenterPluginsDto>>.Error(new NotFoundError(errorMessage));
            }

            // Retrieve PLugin
            var filterPlugin = ExpressionFilterFactory.Create<Plugin>().WithCode(request.PluginCode);
            var plugin = await _pluginRepository.FirstOrDefaultAsync(filterPlugin, cancellationToken: cancellationToken);
            if (null == plugin)
            {
                var errorMessage = $"Don't find plugin with code '{request.PluginCode}'";
                _logger.LogWarning(errorMessage);
                return Payload<IEnumerable<DatacenterPluginsDto>>.Error(new NotFoundError(errorMessage));
            }

            // Retrieve Credential
            var filterCredential = ExpressionFilterFactory.Create<Credential>().WithName(request.CredentialName);
            var credential = await _credentialRepository.FirstOrDefaultAsync(filterCredential, cancellationToken: cancellationToken);
            if (null == credential)
            {
                var errorMessage = $"Don't find credential with name '{request.CredentialName}'";
                _logger.LogWarning(errorMessage);
                return Payload<IEnumerable<DatacenterPluginsDto>>.Error(new NotFoundError(errorMessage));
            }

            if (request.Action == SetDatacenterPluginEndpointRequest.PluginEndpointAction.Add)
            {
                // Add plugin endpoint
                datacenter.AddPluginEndpoint(plugin, credential);
            }
            else
            {
                // Add plugin endpoint
                datacenter.RemovePluginEndpoint(plugin, credential);
            }

            // Save entity
            var nbChanges = await _datacenterRepository.UpdateAsync(datacenter, cancellationToken: cancellationToken);
            if (nbChanges > 0)
                _logger.LogInformation("Succefully {0} plugin {1} endpoint with credential {2} for datacenter {3}", 
                    request.Action,
                    request.PluginCode, request.CredentialName, request.DatacenterCode);


            // return datacenter with plugin endpoint
            return Payload<IEnumerable<DatacenterPluginsDto>>.Success(datacenter.ToDatacenterPLugingsDtoCollection());

        }
    }

}
