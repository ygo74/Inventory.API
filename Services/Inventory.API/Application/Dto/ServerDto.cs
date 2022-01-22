using Inventory.Domain.Models;
using Inventory.Domain.Models.Configuration;
using System.Collections.Generic;

namespace Inventory.API.Application.Dto
{
    public class ServerDto
    {
        public string HostName { get; set; }
        public int CPU { get; set; }
        public int RAM { get; set; }
        public System.Net.IPAddress Subnet { get; set; }

        public List<Environment> Environments { get; set; }

        public string Status { get; set; }
        public Domain.Models.Configuration.OperatingSystem OperatingSystem { get; private set; }

        public Location Location { get; private set; }

        public Dictionary<string, object> Variables { get; set; }
    }
}
