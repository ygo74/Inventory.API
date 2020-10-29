using Ardalis.Specification;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
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

    }
}
