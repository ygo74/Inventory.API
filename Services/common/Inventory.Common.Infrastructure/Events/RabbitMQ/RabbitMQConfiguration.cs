using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Infrastructure.Events.RabbitMQ
{
    public class RabbitMQConfiguration
    {

        public const string SectionName = "RabbitMQ";

        public string HostName { get; set; }
        public string VirtualHost { get; set; } = "/";

        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";

        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; } = "direct";

        public string QueueName { get; set; }
        public string QueueType { get; set; } = "direct";

        public string RoutingKey { get; set; }

        // retry
        public int RetryCount { get; set; } = 5;

        // state
        public bool IsEnabled { get; set; } = false;
    }
}
