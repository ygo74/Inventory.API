using Inventory.Common.DomainModels;
using Inventory.Domain.Models.ManagedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Domain.Models.Configuration
{
    public class Location : ConfigurationEntity
    {
        public string CountryCode { get; private set; }
        public string CityCode { get; private set; }
        public string Name { get; private set; }

        // Server links
        private List<Server> _servers = new List<Server>();
        public ICollection<Server> Servers => _servers.AsReadOnly();

        public Location(string name, string countryCode, string cityCode)
        {
            Name = !String.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));
            CountryCode = !String.IsNullOrEmpty(countryCode) ? countryCode : throw new ArgumentNullException(nameof(countryCode));
            CityCode = !String.IsNullOrEmpty(cityCode) ? cityCode : throw new ArgumentNullException(nameof(cityCode));
        }

        public void AddServer(Server server)
        {
            var existingServer = Servers.FirstOrDefault(s => s.HostName == server.HostName);
            if (null == existingServer)
            {
                _servers.Add(server);
            }
        }

    }
}
