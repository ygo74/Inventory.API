using Inventory.Domain.Models;
using System.Collections.Generic;

namespace Inventory.API.Application.Dto
{
    public class ServerDto
    {
        public string HostName { get; set; }
        public int CPU { get; private set; }
        public int RAM { get; private set; }
        public System.Net.IPAddress Subnet { get; private set; }

        private List<Environment> _environments = new List<Environment>();
        public List<Environment> Environments => _environments;

        public ServerStatus Status { get; private set; }
        public Domain.Models.OperatingSystem OperatingSystem { get; private set; }

        private List<Group> _groups = new List<Group>();
        public List<Group> Groups => _groups;

        public Dictionary<string, object> Variables { get; set; }
    }
}
