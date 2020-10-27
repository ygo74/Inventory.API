using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Models
{
    public class Environment
    {
        public int    EnvironmentId { get; private set; }
        public string Name { get; private set; }
        public bool   IsProduction { get; private set; }

        // Many to Many Environments
        private List<ServerEnvironment> _serverEnvironments = new List<ServerEnvironment>();
        public ICollection<ServerEnvironment> ServerEnvironments => _serverEnvironments.AsReadOnly();


        public Environment(string name, bool isProduction=false)
        {
            Name = !String.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));
            IsProduction = isProduction;
        }


    }
}
