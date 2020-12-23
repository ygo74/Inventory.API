using System;
using System.Collections.Generic;
using System.Reflection;
using GraphQL.DataLoader;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Inventory.Infrastructure.Databases;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Models;
using Inventory.Infrastructure.Databases.Repositories;
using GraphQL.Utilities.Federation;
using OperatingSystem = Inventory.Domain.Models.OperatingSystem;
using Inventory.Domain;
using Inventory.API.Infrastructure;
using Inventory.API.Graphql.Types;
using Inventory.Infrastructure.GroupVarsFiles;
using AutoMapper;
using MediatR;
using Inventory.API.Graphql;
using Inventory.API.Commands.Application.Behaviors;
using Inventory.API.Commands;
using FluentValidation;
using Inventory.API.Graphql.Extensions;

namespace Inventory.API
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAutoMapper(typeof(Startup));
            services.AddMemoryCache();
            services.AddMediatR(typeof(Startup));
            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<ServiceFactory>(p => p.GetService);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

            services.AddCustomDbContext(Configuration);

            services.AddSwagger()
                    .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.AddTransient<IValidator<CreateServerCommand>, CreateServerValidator>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


            services.AddScoped<IAsyncRepository<Location>, EfRepository<Location>>()
                    .AddScoped<IAsyncRepository<TrustLevel>, EfRepository<TrustLevel>>()
                    .AddScoped<IAsyncRepository<OperatingSystem>, EfRepository<OperatingSystem>>()
                    .AddScoped<IAsyncRepository<Domain.Models.Environment>, EfRepository<Domain.Models.Environment>>();

            services.AddScoped<IAsyncRepository<Server>, EfRepository<Server>>();
            services.AddScoped<IAsyncRepository<Group>, EfRepository<Group>>();
            services.AddScoped<IAsyncRepository<ServerGroup>, EfRepository<ServerGroup>>();
            services.AddScoped<IGroupRepository, GroupRepository >();

            services.AddScoped<InventoryService >();
            services.AddScoped<InventoryFilesContext>();
            services.AddScoped<GraphQLService>();

            services.ResolveGraphDependencies(Environment);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {

            // If, for some reason, you need a reference to the built container, you
            // can use the convenience extension method GetAutofacRoot.
            //this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger()
               .UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory.API V1");
              });

            //app.UseGraphQL<InventorySchema, GraphQLHttpMiddlewareWithLogs<InventorySchema>>("/graphql");
            app.UseGraphQL<InventorySchema>("/graphql");

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path = "/ui/playground",
                BetaUpdates = true,
                RequestCredentials = RequestCredentials.Omit,
                HideTracingResponse = false,

                EditorCursorShape = EditorCursorShape.Line,
                EditorTheme = EditorTheme.Light,
                EditorFontSize = 14,
                EditorReuseHeaders = true,
                EditorFontFamily = "Consolas",

                PrettierPrintWidth = 80,
                PrettierTabWidth = 2,
                PrettierUseTabs = true,

                SchemaDisableComments = false,
                SchemaPollingEnabled = true,
                SchemaPollingEndpointFilter = "*localhost*",
                SchemaPollingInterval = 5000,

                Headers = new Dictionary<string, object>
                {
                    ["MyHeader1"] = "MyValue",
                    ["MyHeader2"] = 42,
                },
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // Mapping of endpoints goes here:
                endpoints.MapControllers();
            });


        }
    }

    public static class CustomExtensionMethods 
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<InventoryDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("InventoryDatabase"),
                                  npgsqlOptionsAction: sqlOptions =>
                                  {
                                      sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                      //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                      sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), null);
                                  });
                
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Dynamic Inventory",
                    Version = "v1",
                    Description = "Dynamic Inventory"
                });
            });

            return services;

        }


    }

}
