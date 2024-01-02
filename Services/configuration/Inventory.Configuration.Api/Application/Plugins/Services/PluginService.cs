using Ardalis.GuardClauses;
using AutoMapper;
using Inventory.Common.Application.Plugins;
using Inventory.Common.Domain.Filters;
using Inventory.Configuration.Api.Application.Plugins.Dtos;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using Inventory.Plugins.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Plugins.Services
{
    public class PluginService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PluginService> _logger;
        private readonly PluginResolver _pluginResolver;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;
        private readonly IConfiguration _configuration;

        public PluginService(IDbContextFactory<ConfigurationDbContext> factory, 
                             IConfiguration configuration, 
                             IMapper mapper, ILogger<PluginService> logger, PluginResolver pluginResolver)
        {
            _factory = Guard.Against.Null(factory, nameof(factory));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _logger = Guard.Against.Null(logger, nameof(logger));
            _configuration = Guard.Against.Null(configuration, nameof(configuration));
            _pluginResolver = Guard.Against.Null(pluginResolver, nameof(pluginResolver));

        }

        public PluginDto GetPluginDto(Plugin pluginEntity)
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

        public async Task<List<Plugin>> GetAllActivePlugins(CancellationToken cancellationToken = default)
        {
            await using var dbContext = _factory.CreateDbContext();

            // filter
            var filter = ExpressionFilterFactory.Create<Plugin>()
                                                .Valid();

            return await dbContext.Plugins.Where(filter.Predicate).ToListAsync();

        }

        public void RegisterPlugin(Plugin plugin)
        {
            _logger.LogInformation("Register plugin {0} with code {1}", plugin.Name, plugin.Code);
            var assembly = _pluginResolver.LoadPlugin(plugin.Path);
            if (assembly == null)
            {
                _logger.LogWarning("Unable to register plugin {0}, assembly not found from {1}", plugin.Name, plugin.Path);
                return;
            }

            var hasSubnet = assembly.GetTypes().Any(t => typeof(ISubnetProvider).IsAssignableFrom(t));
            if (hasSubnet)
            {
                _logger.LogInformation("Register Interface ISubnetProvider plugin {0} with code {1}", plugin.Name, plugin.Code);
                _pluginResolver.RegisterIntegrationsFromAssembly<ISubnetProvider>(plugin.Code, _configuration, assembly);
            }


        }


    }
}
