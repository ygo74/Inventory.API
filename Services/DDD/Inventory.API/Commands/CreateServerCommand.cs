using Inventory.API.Dto;
using Inventory.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Commands
{
    public class CreateServerCommand : IRequest<ServerDto>
    {
        public String HostName { get; set; }
        public OsFamilly OsFamilly { get; set; }
        public String Os { get; set; }
        public String Environment { get; set; }
        public String SubnetIp { get; set; }
        public IEnumerable<DiskDto> Disks { get; set; }

    }
}
