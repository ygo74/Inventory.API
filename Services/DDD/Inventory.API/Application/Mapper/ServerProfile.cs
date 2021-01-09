using AutoMapper;
using Inventory.API.Application.Dto;
using Inventory.Domain.Models;
using System.Linq;

namespace Inventory.API.Application.Mapper
{
    public class ServerProfile : Profile
    {
        public ServerProfile()
        {
            CreateMap<Server, ServerDto>()
                .ForMember(s => s.Groups, opt =>
                {
                    opt.MapFrom(s => s.ServerGroups.Select(sg => sg.Group));
                })
                .ForMember(s => s.Environments, opt =>
                {
                    opt.MapFrom(s => s.ServerEnvironments.Select(se => se.Environment));
                })
                .ForMember(s => s.Disks, opt => opt.MapFrom(s => s.ServerDisks));
        }
    }
}
