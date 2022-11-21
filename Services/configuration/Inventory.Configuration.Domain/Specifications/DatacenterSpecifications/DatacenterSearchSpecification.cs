using Ardalis.Specification;
using Inventory.Configuration.Domain.Models;
using Inventory.Domain.Base.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Domain.Specifications.DatacenterSpecifications
{
    public class DatacenterSearchSpecification : Specification<Datacenter>
    {
        public DatacenterSearchSpecification()
        {

        }

        public DatacenterSearchSpecification(IExpressionFilter<Datacenter> filter)
        {
            Query.Where(filter.Predicate);
        }

        public DatacenterSearchSpecification(string code)
        {
            Query.Where(e => e.Code == code);
        }

        public DatacenterSearchSpecification(Expression<Func<Datacenter, bool>> expression)
        {
            Query.Where(expression);
        }


    }
}
