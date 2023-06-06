using Inventory.Common.Domain.Repository;
using Inventory.Common.Infrastructure.Database;
using Inventory.Devices.Domain.Models;
using Inventory.Devices.Infrastructure.EntityConfiguration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperatingSystem = Inventory.Devices.Domain.Models.OperatingSystem;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Threading;

namespace Inventory.Devices.Infrastructure
{
    public class ServerDbContext : ApplicationDbContext, IUnitOfWork
    {

        //public ServerDbContext() : base() { }

        public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options) { }

        // Configuration tables
        public DbSet<Server> Servers { get; set; }
        public DbSet<VirtualServer> VirtualServers { get; set; }
        public DbSet<OperatingSystem> OperatingSystems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OperatingSystemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DatacenterEntityTypeConfiguration());
        }

    }

    public class ServerDbContextDesignFactory : IDesignTimeDbContextFactory<ServerDbContext>
    {
        public ServerDbContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Inventory.Devices.Api"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Get connection string
            var optionsBuilder = new DbContextOptionsBuilder<ServerDbContext>();
            var connectionString = config.GetConnectionString("InventoryDatabase");
            optionsBuilder.UseNpgsql(connectionString);

            IServiceCollection services = new ServiceCollection();

            //services.AddMediatR(typeof(ServerDbContext));
            services.AddScoped<IMediator, NoMediator>();
            var serviceProvider = services.BuildServiceProvider();


            return serviceProvider.GetService<ServerDbContext>();
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
