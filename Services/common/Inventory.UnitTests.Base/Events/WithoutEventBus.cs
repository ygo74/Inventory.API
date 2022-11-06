using Inventory.Infrastructure.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.UnitTests.Base.Events
{
    public class WithoutEventBus : IEventBus
    {
        public void Publish<T>(T @event, string eventName = null) where T : IntegrationEvent
        {
        }

        public void Subscribe<T, TH>(string eventName = null)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
        }
    }
}
