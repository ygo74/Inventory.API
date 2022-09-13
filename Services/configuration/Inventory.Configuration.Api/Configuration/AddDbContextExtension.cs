using Inventory.Configuration.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Configuration
{
    public static class AddDbContextExtension
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddEntityFrameworkNpgsql().AddPooledDbContextFactory<ConfigurationDbContext>((serviceProvider, options) =>
            //services.AddEntityFrameworkNpgsql().AddDbContext<ServerDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("InventoryDatabase"),
                                  npgsqlOptionsAction: sqlOptions =>
                                  {
                                      sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                      //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                      sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), null);
                                  });

                options.UseInternalServiceProvider(serviceProvider);

            });

            return services;
        }

    }
}
