using Inventory.Domain.Enums;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.BaseModels
{
    public class ManagedEntity : Entity
    {
        public LifecycleStatus Status { get; set; }
    }
}
