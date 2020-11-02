using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Repositories.Interfaces
{
    public interface IGroupRepository : IAsyncRepository<Group>
    {
        List<Group> GetAllLinkedGroups(String groupName);
        List<Group> GetParentGroups(String groupName);
        List<Group> GetChildrenGroups(String groupName);

    }
}
