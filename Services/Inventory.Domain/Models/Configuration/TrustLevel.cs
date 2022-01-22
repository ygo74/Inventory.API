using Inventory.Domain.BaseModels;
using Inventory.Domain.Models.ManagedEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Models.Configuration
{
    public class TrustLevel : ConfigurationEntity
    {
        public int TrustLevelId { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }

        // Server links
        private List<Server> _servers = new List<Server>();
        public ICollection<Server> Servers => _servers.AsReadOnly();

        public TrustLevel(string name, string code)
        {
            Name = !String.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));
            Code = !String.IsNullOrEmpty(code) ? code : throw new ArgumentNullException(nameof(code));
        }

    }
}
