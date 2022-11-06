using Inventory.Plugins.Base;
using Inventory.Plugins.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Plugins.Azure
{
    public class PluginFactory : IPluginFactory
    {
        public void Configure(IServiceCollection services)
        {
            services.UseInfrastructureAzureService();
        }
    }
}
