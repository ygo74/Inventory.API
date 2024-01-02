using AutoMapper;
using FluentValidation;
using Inventory.Common.Application.Behaviors;
using Inventory.Common.Application.Users;
using Inventory.Configuration.Api.Configuration;
using Inventory.Configuration.Infrastructure;
using Inventory.Common.Domain.Repository;
using Inventory.Common.Infrastructure.Events.RabbitMQ;
using Inventory.Common.Infrastructure.Telemetry;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using System.Diagnostics;
using Inventory.Common.Application.Plugins;
using Inventory.Common.Infrastructure.Http;
using Inventory.Common.Domain.Interfaces;
using Inventory.Common.Infrastructure.Database;
using Inventory.Plugins.Interfaces;
using Inventory.Configuration.Api.Application.Datacenters.Services;
using Inventory.Configuration.Api.Application.Locations.Services;
using Inventory.Configuration.Api.Application.Plugins.Services;
using Inventory.Configuration.Api.Application.Credentials.Services;
using Microsoft.EntityFrameworkCore;
using HotChocolate.Utilities;

namespace Inventory.Configuration.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddLogging();
            services.AddAutoMapper(typeof(Startup));
            services.AddHttpContextAccessor();

            services.AddMediatR(typeof(Startup));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Add database
            services.AddCustomDbContext(Configuration, Environment);
            services.AddScoped(typeof(IAsyncRepositoryWithSpecification<>), typeof(ConfigurationRepositoryWithSpec<>));
            services.AddScoped(typeof(IGenericQueryStore<>), typeof(ConfigurationQueryStore<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(ConfigurationRepository<>));

            // Add Graphql
            services.AddGraphqlServices(Environment);

            // Add authorization
            services.AddAuthorization();

            // Add RabbitMQ
            services.AddRabbitMQService(Configuration);

            services.AddScoped<ICurrentUser, CurrentUser>();

            string sourceName = null;
            services.AddTelemetryService(Configuration, out sourceName);

            //pagination
            services.AddPagination();

            // Application
            //services.AddSingleton<PluginResolver>();
            services.AddSingleton<PluginResolver>(sp =>
            {
                var logger = sp.GetService<ILogger<PluginResolver>>();

                var pluginResolver = new PluginResolver(logger);
                var assembly = pluginResolver.LoadPlugin(@"D:\devel\github\ansible_inventory\Services\plugins\Azure\Inventory.Plugins.Azure\bin\Debug\net6.0\Inventory.Plugins.Azure.dll");

                pluginResolver.RegisterIntegrationsFromAssembly<ISubnetProvider>(Configuration, assembly);              
                return pluginResolver;
            });
            services.AddScoped<PluginService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IDatacenterService, DatacenterService>();
            services.AddScoped<ICredentialService, CredentialService>();

            // Http hosting
            services.UseHttpHostingConfigurationServices(Configuration);

            services.AddControllers();

            // test
            //var pluginResolver = new PluginResolver();
            //var assembly = pluginResolver.LoadPlugin(@"D:\devel\github\ansible_inventory\Services\plugins\Azure\Inventory.Plugins.Azure\bin\Debug\net6.0\Inventory.Plugins.Azure.dll");

            //pluginResolver.RegisterIntegrationsFromAssembly<ISubnetProvider>(services, Configuration, assembly);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseHttpHostingConfiguration(Configuration);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGraphQLEndpoint();
            });

        }

        private Dictionary<string, object> GetPluginConfigurationsFromDatabase(IServiceProvider serviceScopeFactory)
        {

            var pluginConfigurations = new Dictionary<string, object>();

            using (var scope = serviceScopeFactory.CreateScope())
            {
                // Résoudre le DbContext à partir de la portée actuelle
                var dbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                // Utilisez dbContext pour interagir avec la base de données et récupérer les configurations des plugins
                // Exemple simplifié pour illustration
                // dbContext.Set<VotreEntityType>().ToList() ou d'autres opérations nécessaires

                // Exemple d'ajout de configurations fictives
                pluginConfigurations.Add("PluginConfig1", "Configuration1");
                pluginConfigurations.Add("PluginConfig2", "Configuration2");
            }

            return pluginConfigurations;
        }

    }
}
