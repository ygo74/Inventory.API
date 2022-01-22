using GraphQL.DataLoader;
using GraphQL.Server;
using GraphQL.Utilities.Federation;
using GraphQL.Server.Authorization.AspNetCore;
using Inventory.API.Graphql.Mutations;
using Inventory.API.Graphql.Queries;
using Inventory.API.Graphql.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using GraphQL.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.API.Graphql.Extensions
{
    public static class GraphDependenciesExtensions
    {
        public static void ResolveGraphDependencies(this IServiceCollection services, IWebHostEnvironment environment)
        {

            services.AddScoped<InventoryType>()
                    .AddScoped<LocationType>()
                    .AddScoped<TrustLevelType>()
                    .AddScoped<ApplicationType>();
                    
            services.AddScoped<InventoryQuery>()
                    .AddScoped<ServerQuery>()
                    .AddScoped<ServerMutation>()
                    .AddScoped<ConfigurationQuery>()
                    .AddScoped<GetAuthorizationTokenMutation>()
                    .AddScoped<InventorySchema>();


            services.AddScoped<AnyScalarGraphType>()
                    .AddTransient<IValidationRule, AuthorizationValidationRule>()
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
                    .AddGraphQLAuthorization(options =>
                    {
                        //options.AddPolicy("test", policy => policy.RequireAuthenticatedUser());
                        options.AddPolicy("admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                        options.AddPolicy("ansible", policy => policy.RequireClaim(ClaimTypes.Role, "Ansible"));
                    })
                    .AddGraphTypes(typeof(InventorySchema));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var key = Encoding.ASCII.GetBytes("testComplex+valuefrom32;");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        }
    }
}
