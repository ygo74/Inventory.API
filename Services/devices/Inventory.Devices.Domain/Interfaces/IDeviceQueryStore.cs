using Inventory.Common.Domain.Repository;
using Inventory.Devices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Devices.Domain.Interfaces
{
    public interface IDeviceQueryStore : IGenericQueryStore<Device>
    {
        Task<IEnumerable<Device>> GetDevicesByDatacenterAsync(string datacenterCode);
        Task<IEnumerable<Device>> GetDevicesByDatacenterAsync(int datacenterId);
        Task<IEnumerable<Device>> GetDevicesByOperatingSystemAsync(string osCode);
        Task<IEnumerable<Device>> GetDevicesByOperatingSystemAsync(int osId);
    }
}
