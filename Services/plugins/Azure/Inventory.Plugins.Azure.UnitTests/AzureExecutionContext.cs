using Inventory.Common.UnitTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Plugins.Azure.UnitTests
{
    public class AzureExecutionContext : TestExecutionContext<Inventory.Plugins.Azure.Services.AzSubnetProvider>
    {
        public override void Configure(ServiceCollection services, IWebHostEnvironment Environment)
        {
            base.Configure(services, Environment);
            services.UseInfrastructureAzureService();
        }
    }
}
