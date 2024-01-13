using Inventory.Common.Domain.Repository;
using Inventory.Devices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Devices.Domain.Interfaces
{
    public interface IDeviceQueryStore : IGenericQueryStore<Server>
    {
        Task<IEnumerable<Server>> GetDevicesByDatacenterAsync(string datacenterCode);
        Task<IEnumerable<Server>> GetDevicesByDatacenterAsync(int datacenterId);
        Task<IEnumerable<Server>> GetDevicesByOperatingSystemAsync(string osCode);
        Task<IEnumerable<Server>> GetDevicesByOperatingSystemAsync(int osId);
    }
}
