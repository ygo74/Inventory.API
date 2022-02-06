using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly.Extensions.Http;

namespace Inventory.Infrastructure.Network
{
    public static class ServicesExtension
    {
        public static void UseInfrastructureService(this ServiceCollection services)
        {
            //ConfigureServices()  - Startup.cs
            services.AddHttpClient<AzureProvider>()
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
