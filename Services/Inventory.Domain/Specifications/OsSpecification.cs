using Ardalis.Specification;
using Inventory.Domain.Enums;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Specifications
{
    public class OsSpecification : Specification<Models.Configuration.OperatingSystem>
    {
        public OsSpecification(string name)
        {
            Query
                .Where(g => g.Model == name.ToLower());
        }

        public OsSpecification(OsFamily osFamilly, string name)
        {
            Query
                .Where(g => g.Family == osFamilly & g.Model == name.ToLower());
        }

    }
}
