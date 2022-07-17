using Inventory.Infrastructure.Databases;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Graphql
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();
            try
            {
                var host = CreateHostBuilder(configuration, args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var logger = services.GetRequiredService<ILogger<InventoryContextSeed>>();

                    var context = services.GetService<InventoryDbContext>();

                    context.Database.EnsureCreated();
                    context.Database.Migrate();

                    new InventoryContextSeed().SeedAsync(context, logger).Wait();

                }

                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                //Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                //Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(IConfiguration configuration, string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        //.UseSerilog()
                        .UseStartup<Startup>()
                        .UseConfiguration(configuration);
                });
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

    }
}
