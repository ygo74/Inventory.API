using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Base.Telemetry
{
    public static class TelemetryExtensions
    {
        public static IServiceCollection AddTelemetryService(
           this IServiceCollection serviceCollection,
           IConfiguration Configuration, out string source_name)
        {

            //var cfgOption = Configuration.GetSection(nameof(TelemetryOptions)).
            //    .Get<TelemetryOptions>();

            //if (cfgOption is null || string.IsNullOrWhiteSpace(cfgOption?.SourceName))
            //{
            //    throw new ArgumentNullException(nameof(TelemetryOptions), "Options not found or value is incorrect!");
            //}

            serviceCollection.Configure<TelemetryOptions>(
                opt => opt.SourceName = "test" //cfgOption.SourceName
            ) ; 
            //source_name = new ActivitySource(cfgOption.SourceName).Name;
            source_name = new ActivitySource("test").Name;

            serviceCollection.AddSingleton<ITelemetry, Telemetry>();

            return serviceCollection;
        }

    }
}
