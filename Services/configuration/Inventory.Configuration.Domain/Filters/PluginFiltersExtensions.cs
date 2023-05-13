using Inventory.Configuration.Domain.Models;
using Inventory.Common.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Domain.Filters
{
    public static class PluginFiltersExtensions
    {
        public static IExpressionFilter<Plugin> WithCode(this IExpressionFilter<Plugin> filter, string code)
        {
            return filter.And(e => e.Code == code);
        }

        public static IExpressionFilter<Plugin> WithName(this IExpressionFilter<Plugin> filter, string name)
        {
            return filter.And(e => e.Name == name);
        }

    }
}
