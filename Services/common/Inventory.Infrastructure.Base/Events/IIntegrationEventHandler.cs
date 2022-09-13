using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Base.Events
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
