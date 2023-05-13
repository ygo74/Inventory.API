using Ardalis.Specification;
using Inventory.Devices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Devices.Domain.Specifications
{
    public class ServerSpecification : Specification<Server>
    {
        public ServerSpecification() 
        {
            //Query.Include(s => s.OperatingSystem);
        }

        public ServerSpecification(string hostName):this()
        {
            Query
                .Where(s => s.Hostname == hostName.ToLower());
        }

        public ServerSpecification(int Id) : this()
        {
            Query
                .Where(s => s.Id == Id);
        }

    }
}
