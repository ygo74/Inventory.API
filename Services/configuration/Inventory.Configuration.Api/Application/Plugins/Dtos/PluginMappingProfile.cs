using AutoMapper;

namespace Inventory.Configuration.Api.Application.Plugins.Dtos
{
    public class PluginMappingProfile : Profile
    {
        public PluginMappingProfile()
        {
            CreateMap<CreatePluginRequest, Domain.Models.Plugin>();
            CreateMap<Domain.Models.Plugin, PluginDto>();

        }
    }
}
