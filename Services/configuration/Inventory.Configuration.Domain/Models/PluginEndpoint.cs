using Inventory.Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Domain.Models
{
    public class PluginEndpoint : AuditEntity
    {
        public Credential Credential { get; private set; }
        public Plugin Plugin { get; private set; }
        public Dictionary<string, Object> PropertyBag { get; private set; }

        public PluginEndpoint() { }

        public PluginEndpoint SetCredential(Credential credential)
        {
            Credential = credential;
            return this;
        }

        public PluginEndpoint SetPlugin(Plugin plugin)
        {
            Plugin = plugin;
            return this;
        }

    }
}
