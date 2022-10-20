using AutoMapper;
using FluentValidation;
using Inventory.Api.Base.Behaviors;
using Inventory.Api.Base.Users;
using Inventory.Domain.Base.Repository;
using Inventory.Infrastructure.Base.Events;
using Inventory.Infrastructure.Base.Events.RabbitMQ;
using Inventory.Infrastructure.Base.Telemetry;
using Inventory.Devices.Api.Configuration;
using Inventory.Devices.Api.IntegrationEvents;
using Inventory.Devices.Infrastructure;
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

namespace Inventory.Devices.Api
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
            services.AddCustomDbContext(Configuration,Environment);
            services.AddScoped(typeof(IAsyncRepository<>), typeof(DevicesRepository<>));

            // Add Graphql
            services.AddGraphqlServices(Environment);

            // Add RabbitMQ
            services.AddRabbitMQService(Configuration);
            services.AddTransient<DatacenterIntegrationEventHandler>();


            services.AddScoped<ICurrentUser, CurrentUser>();
            string sourceName = null;
            services.AddTelemetryService(Configuration, out sourceName);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory.Devices.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory.Devices.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGraphQLEndpoint();
            });

            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<DatacenterIntegrationEvent, DatacenterIntegrationEventHandler>("dctest");
        }

    }
}
