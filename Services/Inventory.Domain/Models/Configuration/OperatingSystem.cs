using Inventory.Domain.BaseModels;
using Inventory.Domain.Enums;
using Inventory.Domain.Models.ManagedEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Domain.Models.Configuration
{
    public class OperatingSystem : ConfigurationEntity
    {

        [Required]
        public String Model { get; private set; }

        public OsFamily Family { get; private set; }

        public string Version { get; private set; }


        // Server links
        private List<Server> _servers = new List<Server>();
        public ICollection<Server> Servers => _servers.AsReadOnly();

        protected OperatingSystem()
        {
        }

        public OperatingSystem(OsFamily family, String model, string version)
        {
            Family = family;
            Model = !String.IsNullOrWhiteSpace(model) ? model.ToLower() : throw new ArgumentNullException(nameof(model));
            Version = !String.IsNullOrWhiteSpace(version) ? version.ToLower() : throw new ArgumentNullException(nameof(version));
        }

    }
}
