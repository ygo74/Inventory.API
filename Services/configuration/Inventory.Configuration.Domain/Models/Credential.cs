using Ardalis.GuardClauses;
using Inventory.Common.Domain.Models;
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
        public Dictionary<string, Object> PropertyBag { get; private set; }

        protected Credential() { }
        public Credential(string name, string description)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            PropertyBag = new Dictionary<string, object>();
        }

    }
}
