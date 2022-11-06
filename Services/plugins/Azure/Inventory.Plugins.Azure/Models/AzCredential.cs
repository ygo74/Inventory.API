using Ardalis.GuardClauses;
using System;

namespace Inventory.Plugins.Azure.Models
{
    public class AzCredential
    {
        public string UserName { get; private set; }
        public string Description { get; private set; }
        public string Password { get; private set; }
        public Guid TenantId { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid SubscriptionId { get; private set; }

        protected AzCredential() { }

        public AzCredential(string name, string description)
        {
            UserName = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            if (!string.IsNullOrWhiteSpace(description))
                Description = description;

        }

        public void SetAzurePassword(Guid subsscriptionId, Guid tenantId, Guid clientId, string password)
        {
            SubscriptionId = Guard.Against.Null(subsscriptionId, nameof(subsscriptionId));
            TenantId = Guard.Against.Null(tenantId, nameof(tenantId));
            ClientId = Guard.Against.Null(clientId, nameof(clientId));
            Password = Guard.Against.NullOrWhiteSpace(password, nameof(password));
        }

    }
}
