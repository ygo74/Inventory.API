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

        public const string DATACENTER_PARIS_CODE = "EMEA-FR-PARIS";
        public const string DATACENTER_PARIS_NAME = "Paris";
        public const string DATACENTER_PARIS_INVENTORY_CODE = "dc.paris";

        public const string DATACENTER_LONDON_CODE = "EMEA-UK-LDN";
        public const string DATACENTER_LONDON_NAME = "London";
        public const string DATACENTER_LONDON_INVENTORY_CODE = "dc.london";


        public static IEnumerable<Datacenter> Get()
        {

            var dbContext = UnitTestsContext.Current.GetService<ConfigurationDbContext>();
            
            // Locations
            var parisLocation = dbContext.Locations.First(e => e.Name == LocationSeed.Name_Paris);
            var LondonLocation = dbContext.Locations.First(e => e.Name == LocationSeed.Name_London);

            // Plugin Endpoints
            var AzPlugin = dbContext.Plugins.First(e => e.Name == PluginSeed.AZURE_INVENTORY);
            var adminCred = dbContext.Credentials.First(e => e.Name == CredentialSeed.ADMINISTRATOR);


            yield return new Datacenter(
                code: DATACENTER_PARIS_CODE, 
                name: DATACENTER_PARIS_NAME, 
                dataCenterType: DatacenterType.OnPremise,
                inventoryCode: DATACENTER_PARIS_INVENTORY_CODE, 
                description: "Main datacenter"
                )
                .SetLocation(parisLocation)
                .AddPluginEndpoint(AzPlugin, adminCred);

            yield return new Datacenter(
                code: DATACENTER_LONDON_CODE,
                name: DATACENTER_LONDON_NAME,
                dataCenterType: DatacenterType.OnPremise,
                inventoryCode: DATACENTER_LONDON_INVENTORY_CODE,
                description: "DRP datacenter"            
                )
                .SetLocation(LondonLocation)
                .AddPluginEndpoint(AzPlugin, adminCred);

        }

    }
}
