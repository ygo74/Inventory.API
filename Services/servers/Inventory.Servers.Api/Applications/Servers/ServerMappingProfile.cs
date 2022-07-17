using AutoMapper;
using Inventory.Servers.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Servers.Api.Applications.Servers
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
