using Inventory.Common.Application.Dto;

namespace Inventory.Configuration.Api.Application.Locations
{
    public class LocationDto : ConfigurationEntityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string CountryCode { get; set; }
        public string CityCode { get; set; }
        public string RegionCode { get; set; }

    }


}
