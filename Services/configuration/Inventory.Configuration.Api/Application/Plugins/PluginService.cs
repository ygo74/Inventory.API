using Ardalis.GuardClauses;
using AutoMapper;
using Inventory.Common.Application.Plugins;
using Inventory.Configuration.Infrastructure;
using Inventory.Plugins.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Plugin
{
    public class PluginService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PluginService> _logger;
        private readonly PluginResolver _pluginResolver;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;

        public PluginService(IDbContextFactory<ConfigurationDbContext> factory, IMapper mapper, ILogger<PluginService> logger, PluginResolver pluginResolver)
        {
            _factory = Guard.Against.Null(factory, nameof(factory));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _logger = Guard.Against.Null(logger, nameof(logger));
            _pluginResolver = Guard.Against.Null(pluginResolver, nameof(pluginResolver));

        }

        public PluginDto GetPluginDto(Domain.Models.Plugin pluginEntity)
        {
            _logger.LogInformation("Get PluginDto for plugin {0}", pluginEntity.Name);

            var plugin = _mapper.Map<PluginDto>(pluginEntity);

            if (!string.IsNullOrWhiteSpace(pluginEntity.Path) && System.IO.File.Exists(pluginEntity.Path))
            {
                var assembly = _pluginResolver.LoadPlugin(pluginEntity.Path);

                var hasSubnet = assembly.GetTypes().Any(t => typeof(ISubnetProvider).IsAssignableFrom(t));
                plugin.SetCapacity("SubnetProvider", hasSubnet);

            }

            return plugin;
        }

        public async Task<bool> PluginExist(int id, CancellationToken cancellationToken)
        {
            await using var dbContext = _factory.CreateDbContext();

            return await dbContext.Plugins.AnyAsync(e => e.Id == id, cancellationToken);
        }

    }
}
