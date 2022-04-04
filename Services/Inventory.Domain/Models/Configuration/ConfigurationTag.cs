using Inventory.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Models.Configuration
{
    public class ConfigurationTag : ConfigurationEntity
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

    }
}
