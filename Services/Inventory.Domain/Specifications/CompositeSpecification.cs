using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Specifications
{

    public static class SpecificationExtension
    {
        public static ISpecification<T> And<T>(this ISpecification<T> specification, ISpecification<T> right) where T : class
        {


            ((List<Expression<Func<T, bool>>>)right.WhereExpressions).ForEach(expression =>
            {
                ((List<Expression<Func<T, bool>>>)specification.WhereExpressions).Add(expression);
            });

            return specification;

        }

    }

}
