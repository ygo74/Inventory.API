using Inventory.Configuration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.SeedWork
{
    public static class LocationSeed
    {
        public const string Name_Paris = "Paris";
        public const string CityCode_Paris = "PAR";
        public const string CountryCode_France = "FR";
        public const string RegionCode_EMEA = "EMEA";

        public const string Name_London = "London";
        public const string CityCode_London = "LDN";
        public const string CountryCode_UK = "UK";


        public static IEnumerable<Location> Get()
        {
            yield return new Location(Name_Paris, CountryCode_France, CityCode_Paris, RegionCode_EMEA, "emea.fr.par", "Paris City");
            yield return new Location(Name_London, CountryCode_UK, CityCode_London, RegionCode_EMEA, "emea.uk.ldn", "London City");
        }
    }
}
