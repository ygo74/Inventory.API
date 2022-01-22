using Inventory.API.Application.Dto;
using Inventory.Domain.Enums;
using Inventory.Domain.Models;
using MediatR;
using System.Collections.Generic;

namespace Inventory.API.Application.Commands
{
    public class CreateServerCommand : IRequest<ServerDto>
    {
        public string HostName { get; set; }
        public OsFamily OsFamilly { get; set; }
        public string Os { get; set; }
        public string Environment { get; set; }
        public string SubnetIp { get; set; }
        public IEnumerable<DiskDto> Disks { get; set; }
        public IEnumerable<string> Groups { get; set; }

    }
}
