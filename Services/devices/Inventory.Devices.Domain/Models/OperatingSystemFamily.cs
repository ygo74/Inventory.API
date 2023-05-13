using Inventory.Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Devices.Domain.Models
{
    public class OperatingSystemFamily : Enumeration
    {
        // Enumeration values
        public static OperatingSystemFamily Windows = new OperatingSystemFamily(1, "Windows");
        public static OperatingSystemFamily Linux = new OperatingSystemFamily(2, "Linux");
        public static OperatingSystemFamily Aix = new OperatingSystemFamily(3, "Aix");
        public static OperatingSystemFamily Solaris = new OperatingSystemFamily(4, "Solaris");


        public OperatingSystemFamily(int id, string name) : base(id, name) { }
    }
}
