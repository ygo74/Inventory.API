using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.SeedWork
{
    public static class DataCenterSeed
    {
        public static IEnumerable<Datacenter> Get()
        {

            var dbContext = UnitTestsContext.Current.GetService<ConfigurationDbContext>();
            
            // Locations
            var parisLocation = dbContext.Locations.First(e => e.Name == LocationSeed.Name_Paris);
            var LondonLocation = dbContext.Locations.First(e => e.Name == LocationSeed.Name_London);

            // Plugin Endpoints
            var AzPlugin = dbContext.Plugins.First(e => e.Name == PluginSeed.AZURE_INVENTORY);
            var adminCred = dbContext.Credentials.First(e => e.Name == CredentialSeed.ADMINISTRATOR);


            yield return new Datacenter("EMEA-FR-PARIS", "France", DatacenterType.OnPremise, "dc.paris")
                .SetLocation(parisLocation)
                .AddPluginEndpoint(AzPlugin, adminCred);

            yield return new Datacenter("EMEA-UK-LDN", "United Kingdom", DatacenterType.OnPremise, "dc.london")
                .SetLocation(LondonLocation)
                .AddPluginEndpoint(AzPlugin, adminCred);

        }

    }
}
