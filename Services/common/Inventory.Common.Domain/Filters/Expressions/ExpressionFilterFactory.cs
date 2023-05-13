using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Domain.Filters
{
    public static class ExpressionFilterFactory
    {
        public static IExpressionFilter<T> Create<T>()
        {
            return new NullFilterCriteria<T>();
        }
    }
}
