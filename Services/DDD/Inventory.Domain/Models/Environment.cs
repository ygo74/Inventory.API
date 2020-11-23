using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Models
{
    public class Environment
    {
        public int    EnvironmentId { get; private set; }
        public string Name { get; private set; }

        // Environment Familly
        public EnvironmentFamilly EnvironmentFamilly { get; private set; }

        // Many to Many Environments
        private List<ServerEnvironment> _serverEnvironments = new List<ServerEnvironment>();
        public ICollection<ServerEnvironment> ServerEnvironments => _serverEnvironments.AsReadOnly();

        protected Environment()
        { }

        public Environment(string name, EnvironmentFamilly environmentFamilly)
        {
            Name = !String.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));
            EnvironmentFamilly = environmentFamilly;
        }


    }
}
