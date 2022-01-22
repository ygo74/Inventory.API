using Ardalis.Specification;
using Inventory.Domain.Filters;
using Inventory.Domain.Models;
using Inventory.Domain.Models.ManagedEntities;
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

        /// <summary>
        /// Get multiple server according a list of server ids
        /// </summary>
        /// <param name="serverIds"></param>
        public ServerSpecification(IEnumerable<int> serverIds) : this()
        {
            Query.Where(s => serverIds.Contains(s.ServerId));
        }

        public ServerSpecification(ServerFilter filter) : this()
        {
            if (filter.LoadChildren)
            {
                Query.Include(s => s.OperatingSystem);
            }

            if (filter.IsPagingEnabled)
            {
                Query.Skip(filter.Skip);
                Query.Take(filter.Take);
            }

        }

    }
}
