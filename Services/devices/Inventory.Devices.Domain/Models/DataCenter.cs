using Ardalis.GuardClauses;
using Inventory.Common.Domain.Models;

namespace Inventory.Devices.Domain.Models
{
    public class DataCenter : Entity
    {
        public string Name { get; private set; }

        public DataCenter(string name)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        }
    }
}