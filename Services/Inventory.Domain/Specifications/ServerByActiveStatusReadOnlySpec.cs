using Ardalis.Specification;
using Inventory.Domain.Models.ManagedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Specifications
{
    public class ServerByActiveStatusReadOnlySpec : Specification<Server>
    {
        public ServerByActiveStatusReadOnlySpec()
        {
            Query.AsNoTracking()
                 .Where(e => e.Status == Enums.LifecycleStatus.Deployed);
        }
    }
}
