using Inventory.Devices.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Inventory.Devices.Api.Configuration
{
    public static class AddDbContextExtension
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {

            services.AddEntityFrameworkNpgsql().AddPooledDbContextFactory<ServerDbContext>((serviceProvider, options) =>
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

                if (environment.IsDevelopment())
                {
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                }

            });

            return services;
        }

    }
}
