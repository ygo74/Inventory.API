using Ardalis.Specification;
using Inventory.Configuration.Domain.Models;
using Inventory.Common.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Domain.Specifications.PluginSpecifications
{
    public class PluginSearchSpecification : Specification<Plugin>
    {
        public PluginSearchSpecification(IExpressionFilter<Plugin> filter)
        {
            if (null != filter.Predicate)
                Query.Where(filter.Predicate);
        }
    }
}
