using AutoMapper;
using Inventory.Configuration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenter
{
    public class DatacenterMappingProfile : Profile
    {
        public DatacenterMappingProfile()
        {
            CreateMap<CreateDatacenter.Command, Domain.Models.Datacenter>();
            CreateMap<Domain.Models.Datacenter, DatacenterDto>()
                 //.IncludeBase<ConfigurationEntity, ConfigurationEntityDto>();
                 .ForMember(d => d.ValidFrom, options =>
                 {
                     options.PreCondition(src => src.ValidFrom.HasValue);
                     options.MapFrom(src => src.ValidFrom);
                 });

            CreateMap<Domain.Models.Datacenter, DatacenterIntegrationEvent>()
                .ForMember(d => d.Id, options => options.Ignore());

        }
    }
}
