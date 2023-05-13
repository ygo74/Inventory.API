using Inventory.Devices.Domain;
using Inventory.Devices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Devices.UnitTests.SeedWork
{
    public class ServerSeed
    {
        public static List<Device> Get()
        {
            var srv1 = new VirtualServer(hostname: "test", dnsDomain: "test.fr", subnetIP: "10.20.1.0/24");

            return new List<Device>()
            {
                srv1
            };
        }
    }
}
