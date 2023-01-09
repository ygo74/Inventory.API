using Inventory.Configuration.Api.Application.Locations;
using Inventory.Configuration.UnitTests.SeedWork;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.TestCases
{
    internal static class LocationTestCases
    {
        internal static IEnumerable GetCreateLocationWithCorrectValues()
        {
            yield return new CreateLocationRequest()
            {
                Name = "Geneva",
                CityCode = "GVA",
                CountryCode = "CH",
                Description = "Geneva City",
                RegionCode = LocationSeed.RegionCode_EMEA,
                InventoryCode = "ch.gva",
                ValidFrom= new DateTime(2023,01,01),
                ValidTo = new DateTime(2023, 02, 01)

            };
        }

        internal static IEnumerable GetUpdateLocationWithCorrectValues() 
        {
            yield return new TestCaseData(LocationSeed.CityCode_Paris, 
                                          LocationSeed.CountryCode_France,
                                          LocationSeed.RegionCode_EMEA,
                                          "Paris city updated",
                                          "emea.fr.par",
                                          new DateTime(2023, 01, 01),
                                          null);
        }
    }
}
