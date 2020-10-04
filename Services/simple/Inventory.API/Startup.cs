using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using Inventory.Infrastructure;
using Inventory.API.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Inventory.API.Middlewares;
using Inventory.API.Repository;
using GraphQL.Execution;
using Microsoft.Data.Sqlite;

namespace Inventory.API
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddCustomDbContext(Configuration);

        //    services.AddSwagger()
        //            .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        //    services.AddSingleton<IInventoryRepository, InventoryRepository>();

        //    services.AddSingleton<ServerGroupType>();
        //    services.AddSingleton<ServerType>();


        //    services.AddSingleton<InventoryQuery>()
        //            .AddSingleton<InventoryMutation>()
        //            .AddSingleton<InventorySchema>();


        //    services.AddSingleton<IDocumentExecuter, DocumentExecuter>()
        //            .AddSingleton<IDocumentWriter, DocumentWriter>()
        //            .AddSingleton<IDocumentExecutionListener, DataLoaderDocumentListener>()
        //            .AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>()
        //            .AddGraphQL((options, provider) =>
        //            {
        //                options.EnableMetrics = Environment.IsDevelopment();
        //                var logger = provider.GetRequiredService<ILogger<Startup>>();
        //                options.UnhandledExceptionDelegate = ctx => logger.LogError("{Error} occured", ctx.OriginalException.Message);

        //            })
        //            .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { })
        //            .AddDataLoader()
        //            .AddGraphTypes(typeof(InventorySchema));



        //}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomDbContext(Configuration);

            services.AddSwagger()
                    .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IInventoryRepository, InventoryRepository>();

            services.AddScoped<ServerGroupType>();
            services.AddScoped<ServerType>();


            services.AddScoped<InventoryQuery>()
                    .AddScoped<InventoryMutation>()
                    .AddScoped<InventorySchema>();


            services.AddGraphQL((options, provider) =>
                    {
                        options.EnableMetrics = Environment.IsDevelopment();
                        var logger = provider.GetRequiredService<ILogger<Startup>>();
                        options.UnhandledExceptionDelegate = ctx => logger.LogError("{Error} occured", ctx.OriginalException.Message);

                    })
                    .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { })
                    .AddDataLoader()
                    .AddGraphTypes(ServiceLifetime.Scoped);



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
#if !DEBUG
            services.AddEntityFrameworkSqlite().AddDbContext<InventoryDbContext>(options =>
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "MyDb.db" };
                var connectionString = connectionStringBuilder.ToString();
                var connection = new SqliteConnection(connectionString);

                options.UseSqlite(connection);

            }, ServiceLifetime.Scoped);
#else

            services.AddEntityFrameworkNpgsql().AddDbContext<InventoryDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("InventoryDbConnectionString"),
                                  npgsqlOptionsAction: sqlOptions =>
                                  {
                                      sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                      //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                      sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), null);
                                  });

        }, ServiceLifetime.Scoped);

#endif

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
