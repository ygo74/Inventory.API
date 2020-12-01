using Inventory.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Domain
{
    public interface IInventoryService
    {
        Task<Server> AddServerAsync(string hostName, OsFamilly osFamilly, string operatingSystemName, string environmentName, System.Net.IPAddress subnetIP);
        Task<Group> GetGroupByIdAsync(int id);
        Task<OperatingSystem> GetorAddOperatingSystemByNameAsync(OsFamilly osFamilly, string name);
        Task<Server> GetServerByIdAsync(int id);
        Task<IReadOnlyList<ServerGroup>> GetServersByGroupAsync(IEnumerable<int> groupIds);
    }
}