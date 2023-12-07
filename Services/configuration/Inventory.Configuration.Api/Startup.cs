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
using Inventory.Configuration.Api.Application.Plugin;
using Inventory.Common.Application.Plugins;
using Inventory.Configuration.Api.Application.Locations;
using Inventory.Configuration.Api.Application.Credentials;
using Inventory.Common.Infrastructure.Http;
using Inventory.Common.Domain.Interfaces;
using Inventory.Common.Infrastructure.Database;
using Inventory.Configuration.Api.Application.Datacenters;

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
            services.AddSingleton<PluginResolver>();
            services.AddScoped<PluginService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<DatacenterService>();
            services.AddScoped<CredentialService>();

            // Http hosting
            services.UseHttpHostingConfigurationServices(Configuration);

            services.AddControllers();
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

    }
}
