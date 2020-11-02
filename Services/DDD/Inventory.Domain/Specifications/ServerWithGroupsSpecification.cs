using Ardalis.Specification;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Domain.Specifications
{
    public class ServerWithGroupsSpecification : Specification<ServerGroup>
    {
        public ServerWithGroupsSpecification()
        {
            Query.Include(sg => sg.Server).ThenInclude(s => s.ServerDisks);
        }

        public ServerWithGroupsSpecification(int groupId):this()
        {
            Query.Where(sg => sg.GroupId == groupId);
        }

        public ServerWithGroupsSpecification(IEnumerable<int> groupIds) : this()
        {
            Query.Where(sg => groupIds.Contains(sg.GroupId));
        }

    }
}
