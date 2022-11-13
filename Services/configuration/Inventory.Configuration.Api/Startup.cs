using AutoMapper;
using FluentValidation;
using Inventory.Api.Base.Behaviors;
using Inventory.Api.Base.Users;
using Inventory.Configuration.Api.Configuration;
using Inventory.Configuration.Infrastructure;
using Inventory.Domain.Base.Repository;
using Inventory.Infrastructure.Base.Events.RabbitMQ;
using Inventory.Infrastructure.Base.Telemetry;
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
using Inventory.Api.Base.Plugins;

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

            // Application
            services.AddSingleton<PluginResolver>();
            services.AddScoped<PluginService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQLEndpoint();
            });

        }

    }
}
