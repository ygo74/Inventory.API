using Inventory.Common.Domain.Interfaces;
using Inventory.Common.Domain.Repository;
using Inventory.Common.Infrastructure.Database;
using Inventory.Provisioning.Domain.Models;
using Inventory.Provisioning.Infrastructure.EntityConfiguration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Provisioning.Infrastructure
{
    public class ProvisioningDbContext : ApplicationDbContext, IUnitOfWork
    {
        public ProvisioningDbContext(DbContextOptions<ProvisioningDbContext> options) : base(options) { }

        // Configuration tables
        public DbSet<ConfigurationSpecification> ConfigurationSpecifications { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<LabelName> LabelNames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigurationSpecificationTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LabelNameTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LabelTypeConfiguration());

        }

    }

    public class ProvisioningDbContextDesignFactory : IDesignTimeDbContextFactory<ProvisioningDbContext>
    {
        public ProvisioningDbContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Inventory.Devices.Api"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddJsonFile("secrets/appsettings.secrets.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            IServiceCollection services = new ServiceCollection();

            // Get connection string
            services.AddDbContext<ProvisioningDbContext>((serviceProvider, options) =>
            {
                var connectionString = config.GetConnectionString("ProvisioningDatabase");
                options.UseNpgsql(connectionString);
            });

            //services.AddMediatR(typeof(ServerDbContext));
            services.AddScoped<IMediator, NoMediator>();
            services.AddScoped<ICurrentUser, NoCurrentUser>();
            var serviceProvider = services.BuildServiceProvider();


            return serviceProvider.GetService<ProvisioningDbContext>();
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
