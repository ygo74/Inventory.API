using Inventory.Configuration.Domain.Models;
using Inventory.Common.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Domain.Filters
{
    public static class DatacenterFiltersExtensions
    {
        public static IExpressionFilter<Datacenter> Valid(this IExpressionFilter<Datacenter> filter, DateTime? date=null)
        {
            var targetedDate = date ?? DateTime.Today;
            return filter.And(e => e.StartDate <= targetedDate && (!e.EndDate.HasValue || e.EndDate >= targetedDate));
        }

        public static IExpressionFilter<Datacenter> WithCode(this IExpressionFilter<Datacenter> filter, string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return filter;
            return filter.And(e => e.Code == code);
        }

        public static IExpressionFilter<Datacenter> WithName(this IExpressionFilter<Datacenter> filter, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return filter;
            return filter.And(e => e.Name == name);
        }

        public static IExpressionFilter<Datacenter> WithInventoryCode(this IExpressionFilter<Datacenter> filter, string inventoryCode)
        {
            if (string.IsNullOrWhiteSpace(inventoryCode)) return filter;
            return filter.And(e => e.InventoryCode == inventoryCode);
        }

    }
}
