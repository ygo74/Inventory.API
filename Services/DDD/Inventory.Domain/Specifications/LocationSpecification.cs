using Ardalis.Specification;
using Inventory.Domain.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Specifications
{
    public class LocationSpecification : Specification<Location>
    {
        public LocationSpecification(string cityCode)
        {
            Query.Where(e => e.CityCode.Equals(cityCode, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
