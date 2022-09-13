using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
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

            serviceCollection.AddOpenTelemetryTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .AddSource("test")
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                        .AddService(serviceName: "otel-test-xx", serviceVersion: "1.0.0"))
                    .AddHttpClientInstrumentation(opts => opts.RecordException = true)
                    .AddAspNetCoreInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation(e => e.SetDbStatementForText = true)
                    .AddHotChocolateInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("http://opentelemetry-collector:55680");
                        options.TimeoutMilliseconds = 10000;
                        //Export dirrectly to APM
                        //options.Endpoint = new Uri("http://apm-server:8200");
                        //options.Headers = "ApiKey test";
                        //options.BatchExportProcessorOptions = new OpenTelemetry.BatchExportProcessorOptions<Activity>()
                        //{

                        //};
                        //options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                        //options.ExportProcessorType = OpenTelemetry.ExportProcessorType.Simple;
                    });


            });

            return serviceCollection;
        }

    }
}
