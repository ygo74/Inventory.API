using Inventory.Common.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Devices.Api.Applications.Servers
{
    public class ServerDto : AuditEntityDto
    {
        public string Hostname { get; set; }
        public string DatacenterName { get; set; }
        public string OperatingSystemFamily { get; set; }
        public string OperatingSystemModel { get; set; }
        public string OperatingSystemVersion { get; set; }
    }
}
