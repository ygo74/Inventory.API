using Inventory.Common.Domain.Filters;
using Inventory.Configuration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Domain.Filters
{
    public static class LocationFiltersExtensions
    {
        public static IExpressionFilter<Location> WithCityCode(this IExpressionFilter<Location> filter, string code)
        {
            return filter.And(e => e.CityCode == code.ToLower());
        }

        public static IExpressionFilter<Location> WithCountryCode(this IExpressionFilter<Location> filter, string code)
        {
            return filter.And(e => e.CountryCode == code.ToLower());
        }

        public static IExpressionFilter<Location> WithRegionCode(this IExpressionFilter<Location> filter, string code)
        {
            return filter.And(e => e.RegionCode == code.ToLower());
        }

        public static IExpressionFilter<Location> WithName(this IExpressionFilter<Location> filter, string name)
        {
            return filter.And(e => e.Name == name);
        }


    }
}
