using System;
using OperatingSystem = Inventory.Domain.Models.Configuration.OperatingSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Domain.Enums;

namespace Inventory.UnitTests.SeedWork.Configuration
{
    public class OperatingSystemSeed
    {
        public static List<OperatingSystem> Get()
        {
            return new List<OperatingSystem>()
            {
                new OperatingSystem(OsFamily.Windows, "Windows Server", "2019"),
                new OperatingSystem(OsFamily.Windows, "Windows Server", "2016"),
                new OperatingSystem(OsFamily.Linux, "RHEL", "7.8"),
                new OperatingSystem(OsFamily.Linux, "RHEL", "8.0")
            };
        }

    }
}
