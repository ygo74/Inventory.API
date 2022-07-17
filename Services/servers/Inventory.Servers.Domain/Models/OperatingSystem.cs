using Inventory.Domain.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Servers.Domain.Models
{
    public class OperatingSystem : Entity
    {
        public string Model { get; set; }
    }
}
