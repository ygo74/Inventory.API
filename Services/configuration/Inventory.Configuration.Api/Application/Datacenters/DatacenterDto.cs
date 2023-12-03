using Inventory.Common.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenters
{
    public class DatacenterDto : ConfigurationEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }


    }
}
