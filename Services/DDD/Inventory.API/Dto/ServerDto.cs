using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Dto
{
    public class ServerDto
    {
        public String HostName { get; set; }
        public int CPU { get; private set; }
        public int RAM { get; private set; }
        public System.Net.IPAddress Subnet { get; private set; }

        public ServerStatus Status { get; private set; }
        public Domain.Models.OperatingSystem OperatingSystem { get; private set; }

        private List<Group> _groups = new List<Group>();
        public List<Group> Groups => _groups;

        public Dictionary<String, Object> Variables { get; set; }
    }
}
