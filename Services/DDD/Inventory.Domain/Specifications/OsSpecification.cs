using Ardalis.Specification;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Specifications
{
    public class OsSpecification : Specification<Models.OperatingSystem>
    {
        public OsSpecification(string name)
        {
            Query
                .Where(g => g.Name == name.ToLower());
        }

        public OsSpecification(OsFamilly osFamilly, string name)
        {
            Query
                .Where(g => g.Familly == osFamilly & g.Name == name.ToLower());
        }

    }
}
