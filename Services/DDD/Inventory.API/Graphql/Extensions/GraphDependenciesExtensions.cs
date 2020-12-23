using GraphQL.DataLoader;
using GraphQL.Server;
using GraphQL.Utilities.Federation;
using Inventory.API.Graphql.Mutations;
using Inventory.API.Graphql.Queries;
using Inventory.API.Graphql.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Hosting;

namespace Inventory.API.Graphql.Extensions
{
    public static class GraphDependenciesExtensions
    {
        public static void ResolveGraphDependencies(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddScoped<InventoryQuery>()
                    .AddScoped<ServerQuery>()
                    .AddScoped<GroupQuery>()
                    .AddScoped<GroupType>()
                    .AddScoped<InventoryType>()
                    .AddScoped<ServerMutation>()
                    .AddScoped<GroupMutation>()
                    .AddScoped<LocationType>()
                    .AddScoped<TrustLevelType>()
                    .AddScoped<EnvironmentType>()
                    .AddScoped<ConfigurationQuery>()
                    .AddScoped<InventorySchema>();


            services.AddScoped<AnyScalarGraphType>()
                    .AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>()
                    .AddSingleton<DataLoaderDocumentListener>()
                    .AddGraphQL((options, provider) =>
                    {
                        options.EnableMetrics = environment.IsDevelopment();
                        var logger = provider.GetRequiredService<ILogger<Startup>>();
                        options.UnhandledExceptionDelegate = ctx => logger.LogError("{Error} occured", ctx.OriginalException.Message);
                    })
                    .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { })
                    .AddDataLoader()
                    .AddGraphTypes(typeof(InventorySchema));

        }
    }
}
