using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Inventory.Devices.Api.Applications.OperatingSystem.Dto
{

    [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum OperatingSystemFamilyDto
    {
        Windows = 1,
        Linux = 2,
        Aix = 3,
        Solaris = 4
    }
}
