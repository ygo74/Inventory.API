using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure.EntityConfiguration;
using Inventory.Common.Domain.Repository;
using Inventory.Common.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Infrastructure
{
    public class ConfigurationDbContext : ApplicationDbContext, IUnitOfWork
    {
        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : base(options) { }

        // Configuration tables
        public DbSet<Datacenter> Datacenters { get; set; }
        public DbSet<Plugin> Plugins { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Location> Locations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DatacenterEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PluginEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CredentialEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LocationEntityTypeConfiguration());
        }

    }

    public class ConfigurationDbContextDesignFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Inventory.Configuration.Api"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddJsonFile("secrets/appsettings.secrets.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


            IServiceCollection services = new ServiceCollection();

            services.AddDbContext<ConfigurationDbContext>((serviceProvider, options) =>
            {
                var connectionString = config.GetConnectionString("InventoryDatabase");
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IMediator, NoMediator>();
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<ConfigurationDbContext>();

        }

        class NoMediator : IMediator
        {
            public Task Publish(object notification, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
            {
                throw new NotImplementedException();
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<object> Send(object request, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }
    }

}
