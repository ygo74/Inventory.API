using Inventory.Devices.Domain.Models;
using System.Collections.Generic;

namespace Inventory.Devices.UnitTests.SeedWork
{
    public class OperatingSystemSeed
    {

        private static List<OperatingSystem> _allOperatingSystems = new List<OperatingSystem>()
        {
            new OperatingSystem(OperatingSystemFamily.Windows, "Server", "2019"),
            new OperatingSystem(OperatingSystemFamily.Windows, "Server", "2022"),
            new OperatingSystem(OperatingSystemFamily.Windows, "Server Core", "2019"),
            new OperatingSystem(OperatingSystemFamily.Windows, "Server Core", "2022")
        };

        public static List<OperatingSystem> Get()
        {
            return _allOperatingSystems;
        }

    }
}
