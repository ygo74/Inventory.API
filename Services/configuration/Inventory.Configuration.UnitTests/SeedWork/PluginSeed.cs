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
        public const string AZURE_INVENTORY_VERSION = "1.0";
        public const string EFFICIENTIP_INVENTORY = "EfficientIP.Inventory";
        public const string EFFICIENTIP_INVENTORY_VERSION = "1.0";

        public static IEnumerable<Plugin> Get()
        {

            yield return new Plugin(AZURE_INVENTORY, AZURE_INVENTORY, AZURE_INVENTORY_VERSION,"az.plugin");
            yield return new Plugin(EFFICIENTIP_INVENTORY, EFFICIENTIP_INVENTORY, EFFICIENTIP_INVENTORY_VERSION,"efficientip.plugin");

        }

    }
}
