using Ardalis.Specification;
using Inventory.Domain.Enums;
using Inventory.Domain.Models.ManagedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Specifications
{
    public class ServerByOSReadOnlySpec : Specification<Server>
    {
        public ServerByOSReadOnlySpec(OsFamily family)
        {
            Query.AsNoTracking()
                 .Where(e => e.OperatingSystem.Family == family)
                 .OrderBy(e => e.OperatingSystem.Model)
                    .ThenBy(e => e.OperatingSystem.Version)
                        .ThenBy(e => e.HostName);
        }

        public ServerByOSReadOnlySpec(OsFamily family, string model):this(family)
        {
            Query.Where(e => e.OperatingSystem.Model.Equals(model, StringComparison.InvariantCultureIgnoreCase));
        }

        public ServerByOSReadOnlySpec(OsFamily family, string model, string version) : this(family, model)
        {
            Query.Where(e => e.OperatingSystem.Version.Equals(version, StringComparison.InvariantCultureIgnoreCase));
        }

    }
}
