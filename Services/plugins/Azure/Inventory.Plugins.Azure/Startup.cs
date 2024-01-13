//using Inventory.Common.Plugins;
using Inventory.Plugins.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Plugins.Azure
{
    public class Startup
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.UseInfrastructureAzureService();
        }
    }
}
