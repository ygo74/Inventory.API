using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Domain.Models
{
    public class OperatingSystem
    {
        public int OperatingSystemId { get; private set; }

        [Required]
        public String Name { get; private set; }

        public OsFamilly Familly { get; private set; }


        // Server links
        private List<Server> _servers = new List<Server>();
        public ICollection<Server> Servers => _servers.AsReadOnly();

        public OperatingSystem()
        { 
        }

        public OperatingSystem(String name, OsFamilly osFamilly=OsFamilly.Windows)
        {
            Name    = !String.IsNullOrWhiteSpace(name) ? name.ToLower() : throw new ArgumentNullException(nameof(name));
            Familly = osFamilly;
        }

    }
}
