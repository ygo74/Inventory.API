using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Infrastructure.Http
{
    public class HttpHostingConfiguration
    {
        public const string ConfigurationSection = "HttpHostingConfiguration";

        public bool? UseReverseProxy { get; set; }

        public string ProxyBasePath { get; set; }

        public string ProxyScheme { get; set; }

        public bool? EnableHttpHeadersLogging { get; set; }

    }
}
