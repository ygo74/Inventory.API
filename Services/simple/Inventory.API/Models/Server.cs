using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Models
{
    public class Server
    {
        public int ServerId { get; set; }
        public string Name { get; set; }

        public OsType OperatingSystem { get; set; }

        public List<ServerGroup> Groups { get; set; }

        //public IDictionary<String, Object> Variables { get; set; }

    }
}
