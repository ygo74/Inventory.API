using Inventory.Domain.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Servers.Domain.Models
{
    public class Server : Entity
    {
        public string Hostname { get; set; }

        //public OperatingSystem OperatingSystem { get; set; }
    }

    public class VirtualServer : Server
    {
        public int ProviderId { get; set; }

        //public OperatingSystem OperatingSystem { get; set; }
    }

}
