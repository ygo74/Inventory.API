using AutoMapper;
using Inventory.Common.Infrastructure.Database;
using Inventory.Devices.Domain.Interfaces;
using Inventory.Devices.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Devices.Infrastructure
{
    public class DeviceQueryStore : GenericQueryStore<ServerDbContext, Device>, IDeviceQueryStore
    {

        public DeviceQueryStore(ServerDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }

        public DeviceQueryStore(IDbContextFactory<ServerDbContext> dbContextFactory, IMapper mapper) : base(dbContextFactory, mapper)
        {

        }

        public Task<IEnumerable<Device>> GetDevicesByDatacenterAsync(string datacenterCode)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Device>> GetDevicesByDatacenterAsync(int datacenterId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Device>> GetDevicesByOperatingSystemAsync(string osCode)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Device>> GetDevicesByOperatingSystemAsync(int osId)
        {
            throw new NotImplementedException();
        }
    }
}
