using Inventory.Infrastructure.Base.Database;
using MediatR;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Networks.Infrastructure
{
    public class NetworksDbContextDesignFactory : IDesignTimeDbContextFactory<NetworksDbContext>
    {
        public NetworksDbContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Inventory.Networks.API"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();


            IServiceCollection services = new ServiceCollection();

            services.AddDbContext<NetworksDbContext>((serviceProvider, options) =>
            {
                var connectionString = config.GetConnectionString("InventoryDatabase");
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IMediator, NoMediator>();
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<NetworksDbContext>();

        }
    }
}
