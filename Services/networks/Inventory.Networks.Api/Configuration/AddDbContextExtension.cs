using Inventory.Networks.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Inventory.Networks.Api.Configuration
{
    public static class AddDbContextExtension
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {

            services.AddEntityFrameworkNpgsql().AddPooledDbContextFactory<NetworksDbContext>((serviceProvider, options) =>
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
                options.EnableDetailedErrors(env.IsDevelopment());
                options.EnableSensitiveDataLogging(env.IsDevelopment());
                //options.LogTo(Log.Logger.Information, LogLevel.Information, null);

        });

            return services;
        }

    }
}
