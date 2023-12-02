using Ardalis.GuardClauses;
using Inventory.Common.Domain.Models;
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

        protected Server()
        {
            DeviceType = nameof(Server);
        }

        public Server(string hostname, string dnsDomain, string subnetIP) :
            base(hostname, dnsDomain, subnetIP)
        {
            DeviceType = nameof(Server);
        }

        public void SetOperatingSystem(OperatingSystem os)
        {
            Guard.Against.Null(os, nameof(os));
            OperatingSystem = os;
        }   


    }

    public class VirtualServer : Server
    {
        public int ProviderId { get; set; }

        protected VirtualServer()
        {
            DeviceType = nameof(VirtualServer);
        }

        //public OperatingSystem OperatingSystem { get; set; }
        public VirtualServer(string hostname, string dnsDomain, string subnetIP) :
            base(hostname, dnsDomain, subnetIP)
        {
            DeviceType = nameof(VirtualServer);
        }
    }

    public class NetworkSwitch : Device
    {
        protected NetworkSwitch()
        {
            DeviceType = nameof(NetworkSwitch);

        }

        public NetworkSwitch(string hostname, string dnsDomain, string subnetIP) :
            base(hostname, dnsDomain, subnetIP)
        {
            DeviceType = nameof(NetworkSwitch);
        }
    }

}
