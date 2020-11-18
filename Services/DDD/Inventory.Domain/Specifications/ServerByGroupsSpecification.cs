using Ardalis.Specification;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Domain.Specifications
{
    public class ServerByGroupsSpecification : Specification<ServerGroup>
    {
        public ServerByGroupsSpecification() : base()
        {
            Query.Include(sg => sg.Server).ThenInclude(s => s.ServerEnvironments).ThenInclude(se => se.Environment);
        }

        public ServerByGroupsSpecification(IEnumerable<int> groupIds) : this()
        {
            Query.Where(sg => groupIds.Contains(sg.Group.GroupId));
        }

        public ServerByGroupsSpecification(String[] groupNames) : this()
        {
            Query.Where(sg => groupNames.Contains(sg.Group.Name));
        }
    }
}
