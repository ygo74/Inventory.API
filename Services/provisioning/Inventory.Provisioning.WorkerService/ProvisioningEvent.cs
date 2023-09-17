using Inventory.Common.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Provisioning.WorkerService
{
    public class ProvisioningEvent : IntegrationEvent
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string InventoryCode { get; set; }
        public bool Deprecated { get; set; }
        public bool IsValid { get; set; }

    }

    public class ProvisioningEventHandler : IIntegrationEventHandler<ProvisioningEvent>
    {
        private readonly ILogger<ProvisioningEventHandler> _logger;

        public ProvisioningEventHandler(ILogger<ProvisioningEventHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(ProvisioningEvent @event)
        {

            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }

}
