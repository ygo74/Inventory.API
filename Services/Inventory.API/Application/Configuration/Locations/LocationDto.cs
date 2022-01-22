using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Application.Configuration.Locations
{
    public class LocationDto : ConfigurationEntityDto
    {
        public string CountryCode { get; private set; }
        public string CityCode { get; private set; }
        public string Name { get; private set; }

    }
}
