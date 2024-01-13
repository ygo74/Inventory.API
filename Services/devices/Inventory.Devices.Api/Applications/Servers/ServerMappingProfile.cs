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
            CreateMap<Server, ServerDto>()
               .ForMember(dest => dest.DatacenterName, opt => opt.MapFrom(src => src.DataCenter.Name))
               .ForMember(dest => dest.OperatingSystemFamily, opt => opt.MapFrom(src => src.OperatingSystem.Family))
               .ForMember(dest => dest.OperatingSystemModel, opt => opt.MapFrom(src => src.OperatingSystem.Model))
               .ForMember(dest => dest.OperatingSystemVersion, opt => opt.MapFrom(src => src.OperatingSystem.Version));

            CreateMap<CreateServer.Command, Server>();
        }
    }
}
