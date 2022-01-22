using Inventory.Domain.Enums;
using Inventory.Domain.Models.ManagedEntities;
using OperatingSystem = Inventory.Domain.Models.Configuration.OperatingSystem;
using Environment = Inventory.Domain.Models.Configuration.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.UnitTests.SeedWork.ManagedEntities
{
    public class ServerSeed
    {

        public static List<Server> Get()
        {

            var osWindows = new OperatingSystem(OsFamily.Windows, "Windows", "2019");
            var osLinux = new OperatingSystem(OsFamily.Linux, "RHEL", "7.9");
            var env = new Environment("POC", "Proof of Concept", EnvironmentFamily.Developoment);


            var servers = new List<Server>();

            var srv1 = new Server("srv1", osWindows, env, 2, 4, System.Net.IPAddress.Parse("10.10.10.0"));
            servers.Add(srv1);

            var srv2 = new Server("srv2", osLinux, env, 1, 2, System.Net.IPAddress.Parse("10.10.10.0"));
            srv2.SetLifecycleStatus(LifecycleStatus.Deployed);
            servers.Add(srv2);

            var srv3 = new Server("srv3", osWindows, env, 2, 4, System.Net.IPAddress.Parse("10.10.10.0"));
            srv3.SetLifecycleStatus(LifecycleStatus.Deployed);
            servers.Add(srv3);


            return servers;

        }

    }
}
