using Inventory.Domain.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Devices.Domain.Models
{
    public class Server : Device
    {
        // Operating system link
        public int OperatingSystemId { get; protected set; }
        public OperatingSystem OperatingSystem { get; private set; }

        public Server(string hostname, string dnsDomain, string subnetIP) :
            base(hostname, dnsDomain, subnetIP)
        {
        }


    }

    public class VirtualServer : Server
    {
        public int ProviderId { get; set; }

        //public OperatingSystem OperatingSystem { get; set; }
        public VirtualServer(string hostname, string dnsDomain, string subnetIP) :
            base(hostname, dnsDomain, subnetIP)
        {
        }

    }

}
