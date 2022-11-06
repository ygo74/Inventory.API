using Inventory.Configuration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.SeedWork
{
    public static class PluginSeed
    {

        public const string AZURE_INVENTORY = "Azure.Inventory";
        public const string EFFICIENTIP_INVENTORY = "EfficientIP.Inventory";

        public static List<Plugin> Get()
        {
            var plugins = new List<Plugin>();

            plugins.Add(new Plugin(AZURE_INVENTORY, AZURE_INVENTORY));
            plugins.Add(new Plugin(EFFICIENTIP_INVENTORY, EFFICIENTIP_INVENTORY));

            return plugins;
        }

    }
}
