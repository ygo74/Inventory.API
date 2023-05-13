using AutoMapper;
using Inventory.Configuration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenter
{
    public class ConfigurationEntityProfile : Profile
    {
        public ConfigurationEntityProfile()
        {
            //CreateMap<ConfigurationEntityDto, ConfigurationEntity>();
            //CreateMap<ConfigurationEntity, ConfigurationEntityDto>()
            //     .ForMember(d => d.ValidFrom, options =>
            //     {
            //         options.PreCondition(src => src.ValidFrom.HasValue);
            //         options.MapFrom(src => src.ValidFrom);
            //     });

        }
    }
}
