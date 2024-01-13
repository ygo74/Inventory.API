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

        public static IExpressionFilter<Plugin> WithId(this IExpressionFilter<Plugin> filter, int id)
        {
            return filter.And(e => e.Id == id);
        }

        public static IExpressionFilter<Plugin> WithCode(this IExpressionFilter<Plugin> filter, string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return filter;
            return filter.And(e => e.Code == code);
        }

        public static IExpressionFilter<Plugin> WithName(this IExpressionFilter<Plugin> filter, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return filter;
            return filter.And(e => e.Name == name);
        }

    }
}
