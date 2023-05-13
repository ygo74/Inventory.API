using Inventory.Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Domain.Filters
{
    public static class ConfigurationEntityFilterExtensions
    {

        public static IExpressionFilter<T> ExcludeDeprecated<T>(this IExpressionFilter<T> filter) where T : ConfigurationEntity
        {
            return filter.And(e => !e.Deprecated);
        }

        public static IExpressionFilter<T> Valid<T>(this IExpressionFilter<T> filter) where T : ConfigurationEntity
        {
            return filter.And(e => e.StartDate.CompareTo(DateTime.Today) <= 0)
                         .And(e => !e.EndDate.HasValue || e.EndDate.Value.CompareTo(DateTime.Today) >= 0);
        }

    }
}
