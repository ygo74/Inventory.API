using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Infrastructure.Events.RabbitMQ
{
    public static class AddRabbitMQExtension
    {
        public static IServiceCollection AddRabbitMQService(
           this IServiceCollection services, IConfiguration configuration)
        {

            // read RabbitMQ configuration from appsettings.json
            services.Configure<RabbitMQConfiguration>(configuration.GetSection(RabbitMQConfiguration.SectionName));

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                
                var rabbitMQConfig = sp.GetRequiredService<IOptions<RabbitMQConfiguration>>().Value;

                var factory = new ConnectionFactory()
                {
                    HostName = rabbitMQConfig.HostName,
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrWhiteSpace(rabbitMQConfig.UserName))
                {
                    factory.UserName = rabbitMQConfig.UserName;
                }

                if (!string.IsNullOrWhiteSpace(rabbitMQConfig.Password))
                {
                    factory.Password = rabbitMQConfig.Password;
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, rabbitMQConfig.RetryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<IServiceProvider>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var rabbitMQConfig = sp.GetRequiredService<IOptions<RabbitMQConfiguration>>().Value;

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, rabbitMQConfig);
            }); 
            

            return services;
        }
    }
}
