using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Infrastructure.Logging
{
    public static class LoggingConfiguration
    {
        public static LoggerConfiguration CreateSerilogLoggerConfiguration(IConfiguration configuration, string appName, string environment)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            var logstashUrl = configuration["Serilog:LogstashgUrl"];
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Debug)
                //.WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
                .Enrich.WithProperty("ApplicationContext", appName)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithElasticApmCorrelationInfo()
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                //.WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl)
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
                .WriteTo.Debug()
                .ReadFrom.Configuration(configuration);
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
        {
            //return new ElasticsearchSinkOptions(new Uri(configuration["ConnectionStrings:Elasticsearch"]))
            return new ElasticsearchSinkOptions(new Uri("https://elastic:test.xx.1@es01:9200"))
            {

                TypeName = null,
                /// <summary>
                /// When set to true the sink will register an index template for the logs in elasticsearch.
                /// This template is optimized to deal with serilog events
                /// </summary>
                AutoRegisterTemplate = true,

                CustomFormatter = new EcsTextFormatter(),

                /// <summary>
                /// When using the <see cref="AutoRegisterTemplate"/> feature, this allows to set the Elasticsearch version. Depending on the
                /// version, a template will be selected. Defaults to pre 5.0.
                /// </summary>
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,

                ///<summary>
                /// Connection configuration to use for connecting to the cluster.
                /// </summary>
                ModifyConnectionSettings = configuration => configuration.ServerCertificateValidationCallback(
                    (o, certificate, arg3, arg4) => {
                        return true;
                    }),

                ///<summary>
                /// The index name formatter. A string.Format using the DateTimeOffset of the event is run over this string.
                /// defaults to "logstash-{0:yyyy.MM.dd}"
                /// Needs to be lowercased.
                /// </summary>
                IndexFormat = $"{Assembly.GetEntryAssembly()?.GetName()?.Name?.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                // QueueSizeLimit = 1000,
                // ConnectionTimeout = new TimeSpan(0,5,0), // 5mins
                // BufferFileSizeLimitBytes = 5242880,
                // BufferLogShippingInterval = new TimeSpan(0,5,0), // 5mins

                FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                EmitEventFailure = EmitEventFailureHandling.ThrowException | EmitEventFailureHandling.WriteToSelfLog,
            };
        }


    }
}
