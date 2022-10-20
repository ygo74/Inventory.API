using Inventory.Devices.Domain.Models;
using System.Collections.Generic;

namespace Inventory.Devices.UnitTests.SeedWork
{
    public static class OperatingSystemSeed
    {

        public static List<OperatingSystem> Get()
        {
            return new List<OperatingSystem>()
            {
                new OperatingSystem(OperatingSystemFamily.Windows, "Server", "2019"),
                new OperatingSystem(OperatingSystemFamily.Windows, "Server", "2022"),
                new OperatingSystem(OperatingSystemFamily.Windows, "Server Core", "2019"),
                new OperatingSystem(OperatingSystemFamily.Windows, "Server Core", "2022")
            };
        }


    }
}
