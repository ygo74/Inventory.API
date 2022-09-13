using Inventory.Domain.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Domain.Events
{
    public class DatacenterEvent : Event
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string InventoryCode { get; set; }

        public DatacenterEvent(EntityAction action) : base(action) { }
    }
}
