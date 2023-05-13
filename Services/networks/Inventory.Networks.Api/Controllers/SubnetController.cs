using Inventory.Networks.Domain.Models;
using Inventory.Plugins.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Networks.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubnetController : ControllerBase
    {

        private readonly ILogger<SubnetController> _logger;
        //private readonly PluginResolver _pluginResolver;
        private readonly IServiceProvider _serviceProvider;

        public SubnetController(ILogger<SubnetController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public async Task<IEnumerable<Subnet>> Get()
        {
            var providers = _serviceProvider.GetServices<ISubnetProvider>();
            return await providers.First().ListAllAsync();

        }
    }
}
