using Inventory.Common.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Devices.Api.Applications.OperatingSystem.Dto
{
    public class OperatingSystemDto : ConfigurationEntityDto
    {
        public OperatingSystemFamilyDto OperatingSystemFamily { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
    }
}
