using Inventory.Common.DomainModels;
using Inventory.Domain.Enums;
using Inventory.Domain.Models.ManagedEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Models.Configuration
{
    public class Environment : ConfigurationEntity
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        // Environment Familly
        public EnvironmentFamily EnvironmentFamilly { get; private set; }

        // Server links
        private List<Server> _servers = new List<Server>();
        public ICollection<Server> Servers => _servers.AsReadOnly();

        protected Environment()
        { }

        public Environment(string code, string name, EnvironmentFamily environmentFamilly)
        {
            Code = !String.IsNullOrEmpty(code) ? code : throw new ArgumentNullException(nameof(code));
            Name = !String.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));
            EnvironmentFamilly = environmentFamilly;
        }


    }
}
