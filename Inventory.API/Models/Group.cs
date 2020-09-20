using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public String Name { get; set; }

        public List<ServerGroup> Servers { get; set; }
        //public IDictionary<String, Object> Variables { get; set; }

    }
}
