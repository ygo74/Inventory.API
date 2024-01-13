using AutoMapper;
using Inventory.Configuration.Api.Application.Locations.Dtos;
using Inventory.Configuration.Domain.Models;

namespace Inventory.Configuration.Api.Application.Locations
{
    public class LocationMappingProfile : Profile
    {
        public LocationMappingProfile()
        {
            CreateMap<Location, LocationDto>();
        }
    }
}
