using AutoMapper;
using Inventory.Plugins.Azure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Inventory.Plugins.Azure
{
    public static class ServicesExtension
    {

        public static void RegisterService(ServiceCollection services)
        {
            services.UseInfrastructureAzureService();
        }

        public static void UseInfrastructureAzureService(this IServiceCollection services)
        {

            services.TryAddEnumerable(services.AddAutoMapper(typeof(ServicesExtension)));

            services.AddScoped<AuthenticationTokenService>()
                    .AddScoped<AzSubnetProvider>();

            //ConfigureServices()  - Startup.cs
            services.AddHttpClient<AzSubnetProvider>()
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
                    .AddPolicyHandler(GetRetryPolicy());
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }
    }
}
