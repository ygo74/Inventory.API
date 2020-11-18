using Ardalis.Specification;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Domain.Specifications
{
    public class ServerSpecification : Specification<Server>
    {
        public ServerSpecification()
        {
            Query.Include(s => s.OperatingSystem);
            Query.Include(s => s.ServerDisks);
            Query.Include(s => s.ServerEnvironments);                
        }

        public ServerSpecification(string hostName):this()
        {
            Query
                .Where(s => s.HostName == hostName.ToLower());
        }

        public ServerSpecification(int Id) : this()
        {
            Query
                .Where(s => s.ServerId == Id);
        }

        public ServerSpecification(string[] groupNames, string environment) : this()
        {
            Query.Include(s => s.ServerGroups).ThenInclude(sg => sg.Group);
            Query.Where(s => s.ServerGroups.Any(sg => groupNames.Contains(sg.Group.Name)) 
                        && s.ServerEnvironments.Any(se => se.Environment.Name == environment));
        }

        /// <summary>
        /// Get multiple server according a list of server ids
        /// </summary>
        /// <param name="serverIds"></param>
        public ServerSpecification(IEnumerable<int> serverIds) : this()
        {
            Query.Include(s => s.ServerGroups);
            Query.Where(s => serverIds.Contains(s.ServerId));
        }


    }
}
