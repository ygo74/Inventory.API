using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Domain.Models
{
    public class Server
    {

        public int ServerId { get; private set; }

        [Required]
        [ConcurrencyCheck]
        public string HostName { get; private set; }

        //OS Familly
        public int OperatingSystemId { get; private set; }
        public OperatingSystem OperatingSystem { get; private set; }

        // Server Groups Link
        private List<ServerGroup> _serverGroups;
        public ICollection<ServerGroup> ServerGroups => _serverGroups.AsReadOnly();

        //public List<Variable> Variables { get; set; }

        protected Server()
        {
            _serverGroups = new List<ServerGroup>();
        }

        public Server(String hostName, OperatingSystem operatingSystem) : this()
        {
            HostName = !String.IsNullOrEmpty(hostName) ? hostName.ToLower() : throw new ArgumentNullException(nameof(hostName));
            OperatingSystem = operatingSystem ?? throw new ArgumentNullException(nameof(operatingSystem));
        }



    }
}
