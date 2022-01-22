using Inventory.Domain.Models.ManagedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Models
{
    public class Application
    {
        public int ApplicationId { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }

        // List of Servers
        private List<Server> _servers = new List<Server>();
        public ICollection<Server> Servers => _servers.AsReadOnly();


        public Application()
        { }

        public Application(string name, string code)
        {
            Name = !String.IsNullOrEmpty(name) ? name.ToLower() : throw new ArgumentNullException(nameof(name));
            Code = !String.IsNullOrEmpty(code) ? code.ToUpper() : throw new ArgumentNullException(nameof(code));
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
