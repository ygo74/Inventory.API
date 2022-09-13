using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Base.Events
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
