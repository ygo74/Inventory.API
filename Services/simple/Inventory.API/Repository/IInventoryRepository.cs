using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.API.Repository
{
    public interface IInventoryRepository
    {
        Task<IDictionary<int, Server>> GetServersByIdAsync(IEnumerable<int> serverIds, CancellationToken token);
        Task<ILookup<int, Server>>     GetServersByGroupAsync(IEnumerable<int> groupIds, CancellationToken token);

        // Groups
        Task<Group> GetGroupById(int groupId);
        Task<ILookup<int, Group>> GetgroupsByServerAsync(IEnumerable<int> serverIds, CancellationToken token);
        List<Group> GetParentGroups(String groupName);
        List<Group> GetChildrenGroups(String groupName);

        Group CreateGroup(String Name, String ParentName = null);
    }
}