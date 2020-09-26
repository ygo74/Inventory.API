using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Domain.Models
{
    public class Server : Entity
    {
        public string Name { get; set; }
        public OsType OperatingSystem { get; set; }

        public List<Group> Groups { get; set; }

        //public IDictionary<String, Object> Variables { get; set; }

    }
}
