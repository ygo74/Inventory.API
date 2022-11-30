using Inventory.Common.Infrastructure.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Devices.Api.IntegrationEvents
{
    public class DatacenterIntegrationEvent : IntegrationEvent
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string InventoryCode { get; set; }
        public bool Deprecated { get; set; }
        public bool IsValid { get; set; }

    }

    public class DatacenterIntegrationEventHandler : IIntegrationEventHandler<DatacenterIntegrationEvent>
    {
        private readonly ILogger<DatacenterIntegrationEventHandler> _logger;

        public DatacenterIntegrationEventHandler(ILogger<DatacenterIntegrationEventHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(DatacenterIntegrationEvent @event)
        {

            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }

}
