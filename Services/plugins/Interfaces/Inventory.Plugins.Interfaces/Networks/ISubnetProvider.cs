using Inventory.Networks.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Plugins.Interfaces
{
    public interface ISubnetProvider
    {
        string Name { get; }
        string Description { get; }

        void InitCredential(string userName, string description, Dictionary<string, object> propertyBag);
        Task<List<Subnet>> ListAllAsync();
    }
}
