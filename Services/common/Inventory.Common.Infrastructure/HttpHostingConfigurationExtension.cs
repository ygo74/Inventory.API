using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Infrastructure.Http.Configuration
{
    public static class HttpHostingConfigurationExtension
    {
        /// <summary>
        /// Configure web application hosting with custom Http hosting configuration
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseHttpHostingConfiguration(this IApplicationBuilder app, IConfiguration configuration)
        {

            //Read HttpHostingConfiguration Configuration
            var httpHostingConfiguration = configuration.GetSection(HttpHostingConfiguration.ConfigurationSection).Get<HttpHostingConfiguration>();
            if (null == httpHostingConfiguration) return app;

            // Enable Http Logging
            if (httpHostingConfiguration.EnableHttpHeadersLogging.HasValue && httpHostingConfiguration.EnableHttpHeadersLogging.Value)
                app.UseHttpLogging();

            // If Application is not behind a proxy, we can stop HttpHostingConfiguration
            if (!httpHostingConfiguration.UseReverseProxy.HasValue || !httpHostingConfiguration.UseReverseProxy.Value) return app;

            // When application is behind a reverse proxy, always use forwarded headers
            app.UseForwardedHeaders();

            // Define proxy custom path
            if (!string.IsNullOrWhiteSpace(httpHostingConfiguration.ProxyBasePath) && httpHostingConfiguration.ProxyBasePath != "/")
            {
                app.UsePathBase(httpHostingConfiguration.ProxyBasePath);
                app.Use((context, next) =>
                {
                    // Override scheme with proxy scheme
                    if (!string.IsNullOrWhiteSpace(httpHostingConfiguration.ProxyScheme))
                        context.Request.Scheme = httpHostingConfiguration.ProxyScheme;

                    context.Request.PathBase = new PathString(httpHostingConfiguration.ProxyBasePath);
                    return next(context);
                });
            }

            return app;
        }

        /// <summary>
        /// Register dedicated services for custom Http hosting configuration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static IServiceCollection UseHttpHostingConfigurationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Read HttpHostingConfiguration Configuration
            var httpHostingConfiguration = configuration.GetSection(HttpHostingConfiguration.ConfigurationSection).Get<HttpHostingConfiguration>();
            if (null == httpHostingConfiguration) return services;


            // Enable Http Logging
            if (httpHostingConfiguration.EnableHttpHeadersLogging.HasValue && httpHostingConfiguration.EnableHttpHeadersLogging.Value)
                services.AddHttpLogging(options =>
                {
                    // Todo : Add others options in the configuration to have a full control on http logging behavior
                    options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
                });

            // If Application is not behind a proxy, we can stop HttpHostingConfiguration
            if (!httpHostingConfiguration.UseReverseProxy.HasValue || !httpHostingConfiguration.UseReverseProxy.Value) return services;

            // Enable Forwarded headers service
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });


            return services;
        }
    }
}
