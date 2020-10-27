using Ardalis.Specification;
using Inventory.Domain.Models;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Specifications
{
    public class EnvironmentSpecification : Specification<Environment>
    {
        public EnvironmentSpecification(string name)
        {
            Query
                .Where(e => e.Name == name.ToLower());
        }
    }
}
