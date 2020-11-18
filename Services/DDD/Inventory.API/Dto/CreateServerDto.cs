using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Dto
{
    public class CreateServerDto
    {
        public String HostName { get; set; }
        public OsFamilly OsFamilly { get; set; }
        public String Os { get; set; }
        public String Environment { get; set; }
        public String SubnetIp { get; set; }

    }
}
