using Inventory.Domain.BaseModels;
using Inventory.Domain.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Models.ManagedEntities
{
    public class Subnet : ManagedEntity
    {
        public string CIDR { get; private set; }

        public string ProviderId { get; private set; }

        public string Provider { get; private set; }

        public DataCenter DataCenter { get; private set; }

    }
}
