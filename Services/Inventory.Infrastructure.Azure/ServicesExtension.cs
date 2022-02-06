using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;
using Polly;
using Polly.Extensions.Http;
using Inventory.Infrastructure.Azure.Services;
using AutoMapper;

namespace Inventory.Infrastructure.Azure
{
    public static class ServicesExtension
    {

        public static void RegisterService(ServiceCollection services)
        {
            services.UseInfrastructureAzureService();
        }

        public static void UseInfrastructureAzureService(this ServiceCollection services)
        {

            services.AddAutoMapper(typeof(ServicesExtension));

            services.AddScoped<AuthenticationTokenService>()
                    .AddScoped<NetworkService>();

            //ConfigureServices()  - Startup.cs
            services.AddHttpClient<NetworkService>()
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
