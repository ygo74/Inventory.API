using Inventory.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Models.Credentials
{
    public abstract class Credential : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        protected Credential() { }

        protected Credential(string name, string description)
        {
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
            Description = !string.IsNullOrWhiteSpace(description) ? description : throw new ArgumentNullException(nameof(description));

        }
    }
}
