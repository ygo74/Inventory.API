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
        public string Description { get; private set; }

        public Location Location { get; private set; }

        private List<PluginEndpoint> _plugins = new List<PluginEndpoint>();
        public ICollection<PluginEndpoint> Plugins => _plugins.AsReadOnly();


        public DatacenterType DataCenterType { get; private set; }

        protected Datacenter() { }

        public Datacenter(string code, string name, DatacenterType dataCenterType, string inventoryCode,
                        string description="", bool? deprecated = null, DateTime? startDate = null, DateTime? endDate = null)
            : base(inventoryCode, deprecated, startDate, endDate)
        {
            // Mandatory properties
            Code = !String.IsNullOrEmpty(code) ? code : throw new ArgumentNullException(nameof(code));
            Name = !String.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));

            DataCenterType = dataCenterType;

            // Optional properties
            Description = description;

            AddDomainEvent(new Event<Datacenter>(this, Event<Datacenter>.EntityAction.Added));

        }

        public Datacenter AddPluginEndpoint(Plugin plugin, Credential credential)
        {
            var existingEndpoint = Plugins.FirstOrDefault(e => string.Compare(e.Plugin.Code, plugin.Code, StringComparison.InvariantCultureIgnoreCase) == 0
                                                            && string.Compare(e.Plugin.Version, plugin.Version, StringComparison.InvariantCultureIgnoreCase) == 0
                                                            && string.Compare(e.Credential.Name, credential.Name, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (null == existingEndpoint)
            {
                var newEndpoint = new PluginEndpoint()
                    .SetCredential(credential)
                    .SetPlugin(plugin);

                _plugins.Add(newEndpoint);
                AddDomainEvent(new Event<Datacenter>(this, Event<Datacenter>.EntityAction.Updated));
            }

            return this;
        }

        public Datacenter RemovePluginEndpoint(Plugin plugin, Credential credential)
        {
            var existingEndpoint = Plugins.FirstOrDefault(e => string.Compare(e.Plugin.Code, plugin.Code, StringComparison.InvariantCultureIgnoreCase) == 0
                                                            && string.Compare(e.Plugin.Version, plugin.Version, StringComparison.InvariantCultureIgnoreCase) == 0
                                                            && string.Compare(e.Credential.Name, credential.Name, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (null != existingEndpoint)
            {
                _plugins.Remove(existingEndpoint);
                AddDomainEvent(new Event<Datacenter>(this, Event<Datacenter>.EntityAction.Updated));
            }

            return this;
        }

        public Datacenter SetLocation(Location location) 
        { 
            Location = location;
            AddDomainEvent(new Event<Datacenter>(this, Event<Datacenter>.EntityAction.Updated));
            return this;
        }


        public Datacenter SetDescription(string description)
        {
            var newValue = description;
            if (string.IsNullOrWhiteSpace(newValue))
                newValue = null;
            else
                newValue = newValue.Trim();

            Description = newValue;
            AddDomainEvent(new Event<Datacenter>(this, Event<Datacenter>.EntityAction.Updated));
            return this;
        }

        public Datacenter SetDatacenterType(DatacenterType value)
        {
            DataCenterType = value;
            AddDomainEvent(new Event<Datacenter>(this, Event<Datacenter>.EntityAction.Updated));
            return this;
        }
    }
}
