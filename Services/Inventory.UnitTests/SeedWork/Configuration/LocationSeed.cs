using Inventory.API.Application.Configuration.Locations;
using Inventory.Domain.Models.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.UnitTests.SeedWork.Configuration
{
    public class LocationSeed
    {
        public static List<Location> Get()
        {
            var locations = new List<Location>();

            locations.Add(new Location("Paris", "fr", "par"));
            locations.Add(new Location("Geneva", "ch", "gva"));

            return locations;
        }


        /*
         * 
         * Test cases
         * 
        */
        public class TestCases
        {
            public static IEnumerable GetLocationDtoWithEmptyMandatoryValues()
            {
                yield return new CreateLocation.Command() { Name = "", CityCode = "", CountryCode = "" };
                yield return new CreateLocation.Command() { Name = "   ", CityCode = "  ", CountryCode = "  " };
                yield return new CreateLocation.Command() { Name = null, CityCode = null, CountryCode = null };
            }

            public static IEnumerable GetLocationDtoWithCityCodeAlreadExists()
            {
                yield return new CreateLocation.Command() { Name = "Paris", CityCode = "PAR", CountryCode = "FR" };
            }

            public static IEnumerable GetLocationDtoWithBadValidToDate()
            {
                yield return new CreateLocation.Command() { Name = "Lyon", CityCode = "LYS", CountryCode = "FR", ValidFrom = new DateTime(2022,1,1), ValidTo = new DateTime(2022,1,1) };
                yield return new CreateLocation.Command() { Name = "Lyon", CityCode = "LYS", CountryCode = "FR", ValidFrom = new DateTime(2022, 1, 1), ValidTo = new DateTime(2021, 1, 1) };
            }

            public static IEnumerable GetLocationDtoWithValidValues()
            {
                yield return new CreateLocation.Command(){ Name = "Lyon", CityCode = "LYS", CountryCode="FR" };
            }

        }

    }
}
