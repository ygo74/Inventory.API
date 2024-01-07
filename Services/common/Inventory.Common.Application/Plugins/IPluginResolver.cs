using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Plugins
{
    public interface IPluginResolver
    {
        T GetService<T>(string pluginKey) where T : class;
        Assembly LoadPlugin(string path);
        IEnumerable<T> CreateCommands<T>(Assembly assembly);
        void RegisterIntegrationsFromAssembly<T>(string pluginKey, IConfiguration configuration, Assembly assembly);
        

    }
}
