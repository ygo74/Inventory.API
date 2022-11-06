using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Plugin
{
    public class PluginMappingProfile : Profile
    {
        public PluginMappingProfile()
        {
            CreateMap<CreatePlugin.Command, Domain.Models.Plugin>();
            CreateMap<Domain.Models.Plugin, PluginDto>();

        }
    }
}
