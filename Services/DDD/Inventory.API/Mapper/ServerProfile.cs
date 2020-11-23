using AutoMapper;
using Inventory.API.Dto;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Mapper
{
    public class ServerProfile : Profile
    {
        public ServerProfile()
        {
            CreateMap<Server, ServerDto>()
                .ForMember(s => s.Groups, opt =>
                {
                    opt.MapFrom(s => s.ServerGroups.Select(sg => sg.Group));
                });
        }
    }
}
