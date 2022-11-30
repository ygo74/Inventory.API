using Inventory.Configuration.Domain.Events;
using Inventory.Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Domain.Models
{
    public class Datacenter : ConfigurationEntity
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        public DatacenterType DataCenterType { get; private set; }

        protected Datacenter() { }

        public Datacenter(string code, string name, DatacenterType dataCenterType, DateTime? validFrom=null, DateTime? validTo=null)
        {
            Code = !String.IsNullOrEmpty(code) ? code : throw new ArgumentNullException(nameof(code));
            Name = !String.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));

            DataCenterType = dataCenterType;

            AddDomainEvent(new Event<Datacenter>(this, Event<Datacenter>.EntityAction.Added));

        }

    }
}
