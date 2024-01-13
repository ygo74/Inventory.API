using Inventory.Common.Domain.Filters;
using Inventory.Provisioning.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Provisioning.Domain.Filters
{
    public static class LabelNameFilterExtension
    {
        public static IExpressionFilter<LabelName> WithExactName(this IExpressionFilter<LabelName> filter, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return filter;
            return filter.And(e => e.Name == name);
        }

    }
}
