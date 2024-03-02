using AutoMapper;
using Inventory.Devices.Api.Applications.OperatingSystem.Dto;
using Inventory.Devices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Devices.Api.Applications.OperatingSystem
{
    public class OperatingSystemMappingProfile : Profile
    {
        public OperatingSystemMappingProfile()
        {
            CreateMap<CreateOperatingSystemRequest, Inventory.Devices.Domain.Models.OperatingSystem>();
            CreateMap<Inventory.Devices.Domain.Models.OperatingSystem, OperatingSystemDto>()
                .ForMember(d => d.OperatingSystemFamily, opts =>
                {
                    opts.MapFrom(s => (OperatingSystemFamilyDto)s.Family.Id);
                });

        }
    }

}
