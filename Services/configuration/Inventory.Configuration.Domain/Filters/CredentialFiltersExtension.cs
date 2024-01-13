using Inventory.Common.Domain.Filters;
using Inventory.Configuration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Domain.Filters
{
    public static class CredentialFiltersExtension
    {
        public static IQueryable<Credential> FilterByName(this IQueryable<Credential> query, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return query;

            return query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
        }

        public static IQueryable<Credential> FilterByDescription(this IQueryable<Credential> query, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return query;

            return query.Where(x => x.Description.ToLower().Contains(description.ToLower()));
        }

        public static IQueryable<Credential> FilterByPropertyBag(this IQueryable<Credential> query, string propertyBag)
        {
            if (string.IsNullOrWhiteSpace(propertyBag))
                return query;

            return query.Where(x => x.PropertyBag.ToString().ToLower().Contains(propertyBag.ToLower()));
        }

        public static IExpressionFilter<Credential> WithName(this IExpressionFilter<Credential> filter, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return filter;
            return filter.And(e => e.Name == name);
        }

    }
}
