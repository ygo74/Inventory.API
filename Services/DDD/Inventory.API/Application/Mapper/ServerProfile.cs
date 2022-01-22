using AutoMapper;
using Inventory.API.Application.Dto;
using Inventory.Domain.Models;
using Inventory.Domain.Models.ManagedEntities;
using System.Linq;

namespace Inventory.API.Application.Mapper
{
    public class ServerProfile : Profile
    {
        public ServerProfile()
        {
            CreateMap<Server, ServerDto>();
        }
    }
}
