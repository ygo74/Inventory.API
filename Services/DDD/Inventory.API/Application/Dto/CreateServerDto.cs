using Inventory.Domain.Models;
using System;
using System.Collections.Generic;

namespace Inventory.API.Application.Dto
{
    public class CreateServerDto
    {
        public string HostName { get; set; }
        public OsFamilly OsFamilly { get; set; }
        public string Os { get; set; }
        public string Environment { get; set; }
        public string SubnetIp { get; set; }
        public IEnumerable<DiskDto> Disks { get; set; }
    }
}
