using Ardalis.GuardClauses;
using Inventory.Domain.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Domain.Models
{
    public class Credential : AuditEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Plugin PluginProvider { get; private set; }

        protected Credential() { }
        public Credential(string name, string description, Plugin provider)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            PluginProvider = Guard.Against.Null(provider, nameof(provider));
        }

    }
}
