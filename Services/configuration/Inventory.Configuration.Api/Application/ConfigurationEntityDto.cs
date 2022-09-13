using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application
{
    public abstract class ConfigurationEntityDto
    {
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public string InventoryCode { get; set; }
    }
}
