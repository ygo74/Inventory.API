using Inventory.Common.Infrastructure.Logging;
using Inventory.Devices.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Devices.Api
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            var loggerConfiguration = LoggingConfiguration.CreateSerilogLoggerConfiguration(configuration, AppName, "xxx");
            Log.Logger = loggerConfiguration.CreateLogger();

            try
            {
                var host = CreateHostBuilder(configuration, args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    //var logger = services.GetRequiredService<ILogger<InventoryContextSeed>>();

                    //var context = services.GetService<ServerDbContext>();
                    //context.Database.EnsureCreated();
                    //context.Database.Migrate();
                    var factory = services.GetService<IDbContextFactory<ServerDbContext>>();
                    using (var dbcontext = factory.CreateDbContext())
                    {
                        dbcontext.Database.EnsureCreated();
                        dbcontext.Database.Migrate();
                    }

                    //new InventoryContextSeed().SeedAsync(context, logger).Wait();

                }

                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        //ASPNET CORE 2.2
        //public static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .UseConfiguration(configuration)
        //        .Build();


        public static IHostBuilder CreateHostBuilder(IConfiguration configuration, string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile("secrets/appsettings.secrets.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseSerilog()
                        .UseStartup<Startup>()
                        .UseConfiguration(configuration);
                });

        }



        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("secrets/appsettings.secrets.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }


    }
}
