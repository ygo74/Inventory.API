using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Models.Credentials
{
    public class AzureCredential : Credential
    {
        public string Password { get; private set; }
        public Guid TenantId { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid SubscriptionId { get; private set; }

        protected AzureCredential() : base() { }

        public AzureCredential(string name, string description) : base(name, description)
        {
        }

        public void SetAzurePassword(Guid subsscriptionId, Guid tenantId, Guid clientId, string password)
        {
            SubscriptionId = !subsscriptionId.Equals(Guid.Empty) ? subsscriptionId : throw new ArgumentNullException(nameof(subsscriptionId));
            TenantId = !tenantId.Equals(Guid.Empty) ? tenantId : throw new ArgumentNullException(nameof(tenantId));
            ClientId = !clientId.Equals(Guid.Empty) ? clientId : throw new ArgumentNullException(nameof(clientId));
            Password = !string.IsNullOrWhiteSpace(password) ? password : throw new ArgumentNullException(nameof(password));

        }

    }
}
