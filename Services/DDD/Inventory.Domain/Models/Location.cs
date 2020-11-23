using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Models
{
    public class Location
    {
        public int LocationId { get; private set; }
        public string Name { get; private set; }
        public string CountryCode { get; private set; }
        public string CityCode { get; private set; }

        public Location(string name, string countryCode, string cityCode)
        {
            Name = !String.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));
            CountryCode = !String.IsNullOrEmpty(countryCode) ? countryCode : throw new ArgumentNullException(nameof(countryCode));
            CityCode = !String.IsNullOrEmpty(cityCode) ? cityCode : throw new ArgumentNullException(nameof(cityCode));
        }

    }
}
