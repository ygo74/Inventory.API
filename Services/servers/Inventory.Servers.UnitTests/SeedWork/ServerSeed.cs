using Inventory.Servers.Domain;
using Inventory.Servers.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Servers.UnitTests.SeedWork
{
    public class ServerSeed
    {
        public static List<Server> Get()
        {
            var srv1 = new VirtualServer()
            {
                Hostname = "test"
            };

            return new List<Server>()
            {
                srv1
            };
        }
    }
}
