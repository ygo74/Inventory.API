using Inventory.Configuration.Domain.Models;
using Inventory.Common.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Domain.Filters
{
    public static class PluginEndpointFiltersExtension
    {
        public static IExpressionFilter<PluginEndpoint> WithPluginCode(this IExpressionFilter<PluginEndpoint> filter, string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return filter;
            return filter.And(e => e.Plugin != null && string.Compare(e.Plugin.Code, code, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public static IExpressionFilter<PluginEndpoint> WithPluginVersion(this IExpressionFilter<PluginEndpoint> filter, string version)
        {
            if (string.IsNullOrWhiteSpace(version)) return filter;
            return filter.And(e => e.Plugin != null && string.Compare(e.Plugin.Version, version, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public static IExpressionFilter<PluginEndpoint> WithCredentialName(this IExpressionFilter<PluginEndpoint> filter, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return filter;
            return filter.And(e => e.Credential != null && string.Compare(e.Credential.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

    }
}
