using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Domain.Filters
{
    public interface IExpressionFilterFactory
    {
        IExpressionFilter<T> Create<T>();
    }
}
