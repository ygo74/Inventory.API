using Inventory.Common.Domain.Filters;
using Inventory.Devices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Devices.Domain.Filters
{
    public static class DeviceFilter
    {
        public static IExpressionFilter<Server> WithDatacenter(this IExpressionFilter<Server> filter, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return filter;
            return filter.And(e => e.DataCenter.Name == name);
        }
    }
}
