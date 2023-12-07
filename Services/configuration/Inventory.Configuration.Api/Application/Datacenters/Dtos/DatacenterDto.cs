using Inventory.Common.Application.Dto;
using Inventory.Configuration.Api.Application.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenters.Dtos
{
    public class DatacenterDto : ConfigurationEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DatacenterType { get; set; }
        public string LocationName { get; set; }
        public string LocationCountryCode { get; set; }
        public string LocationCityCode { get; set; }
        public string LocationRegionCode { get; set; }
    }

    public enum DatacenterTypeDto
    {
        Cloud,
        OnPremise
    }
}
