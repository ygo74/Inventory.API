using Elastic.Apm.DiagnosticSource;
using Inventory.Configuration.Infrastructure;
using Inventory.Common.Infrastructure.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace Inventory.Configuration.Api
{
    public static class Program
    {

        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            var loggerConfiguration = LoggingConfiguration.CreateSerilogLoggerConfiguration(configuration, AppName, "xxx");
            Log.Logger = loggerConfiguration.CreateLogger();

            Log.Information("Start application");
            try
            {
                var host = CreateHostBuilder(args).Build();

                var testConnectionString = configuration.GetSection("ConnectionStrings").GetValue<string>("InventoryDatabase");
                Log.Information("Connectionstring {0}", testConnectionString);

                var testConnectionString2 = configuration.GetConnectionString("InventoryDatabase");
                Log.Information("Connectionstring2 {0}", testConnectionString2);


                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    //var logger = services.GetRequiredService<ILogger<InventoryContextSeed>>();

                    //var context = services.GetService<ServerDbContext>();
                    //context.Database.EnsureCreated();
                    //context.Database.Migrate();
                    var factory = services.GetService<IDbContextFactory<ConfigurationDbContext>>();
                    using (var dbcontext = factory.CreateDbContext())
                    {
                        var testConnectionString3 = dbcontext.Database.GetConnectionString();
                        Log.Information("Connectionstring3 {0}", testConnectionString3);

                        dbcontext.Database.EnsureCreated();
                        //dbcontext.Database.Migrate();
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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            //var configuration = GetConfiguration();

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
                        .UseStartup<Startup>();
                        //.UseConfiguration(configuration);
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
