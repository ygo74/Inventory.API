using Ardalis.GuardClauses;
using AutoMapper;
using Inventory.Api.Base.Plugins;
using Inventory.Plugins.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Plugin
{
    public class PluginService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PluginService> _logger;
        private readonly PluginResolver _pluginResolver;

        public PluginService(IMapper mapper, ILogger<PluginService> logger, PluginResolver pluginResolver)
        {
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _logger = Guard.Against.Null(logger, nameof(logger));
            _pluginResolver = Guard.Against.Null(pluginResolver, nameof(pluginResolver));

        }

        public PluginDto GetPluginDto(Domain.Models.Plugin pluginEntity)
        {
            _logger.LogInformation("Get PluginDto for plugin {0}", pluginEntity.Name);

            var plugin = _mapper.Map<PluginDto>(pluginEntity);

            if (!string.IsNullOrWhiteSpace(pluginEntity.Path))
            {
                var assembly = _pluginResolver.LoadPlugin(pluginEntity.Path);

                var hasSubnet = assembly.GetTypes().Any(t => t.IsAssignableFrom(typeof(ISubnetProvider)));
                plugin.SetCapacity("SubnetProvider", hasSubnet);

            }

            return plugin;
        }

    }
}
