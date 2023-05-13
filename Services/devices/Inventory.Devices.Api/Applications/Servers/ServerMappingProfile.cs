using AutoMapper;
using Inventory.Devices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Devices.Api.Applications.Servers
{
    public class ServerMappingProfile : Profile
    {
        public ServerMappingProfile()
        {
            CreateMap<Server, ServerDto>();
            CreateMap<CreateServer.Command, Server>();
        }
    }
}
