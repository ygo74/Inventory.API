using Ardalis.GuardClauses;
using Inventory.Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Devices.Domain.Models
{
    public abstract class Device : AuditEntity
    {

        // Device type
        public string DeviceType { get; protected set; }

        public string Hostname { get; protected set; }
        public string DnsDomain { get; protected set; }
        public string SubnetIP { get; protected set; }

        // DataCenter link
        public int DataCenterId { get; protected set; }
        public DataCenter DataCenter { get; protected set; }

        // Property bags
        public Dictionary<string, object> PropertyBag { get; protected set; }

        protected Device()
        { }

        public Device(string hostname, string dnsDomain, string subnetIP)
        {
            Hostname = Guard.Against.Null(hostname, nameof(hostname));
            DnsDomain = Guard.Against.Null(dnsDomain, nameof(dnsDomain));
            SubnetIP = Guard.Against.Null(subnetIP, nameof(subnetIP));

        }

        public void SetDataCenter(int dataCenterId)
        {
            DataCenterId = dataCenterId;
        }

    }
}
