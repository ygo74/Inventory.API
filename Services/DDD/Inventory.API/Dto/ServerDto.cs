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

        private List<Group> _groups = new List<Group>();
        public List<Group> Groups => _groups;


        public Dictionary<String, Object> Variables { get; set; }
    }
}
