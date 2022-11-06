using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.SeedWork
{
    public static class CredentialSeed
    {
        public static List<Credential> Get()
        {

            var AzPlugin = UnitTestsContext.Current.GetService<ConfigurationDbContext>().Plugins.First(e => e.Name == PluginSeed.AZURE_INVENTORY);

            var credentials = new List<Credential>();

            credentials.Add(new Credential("azureTenant1", "test azure", AzPlugin));

            return credentials;
        }

    }
}
