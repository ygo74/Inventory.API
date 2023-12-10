using AutoMapper;
using Inventory.Configuration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenters.Dtos
{
    public class DatacenterMappingProfile : Profile
    {
        public DatacenterMappingProfile()
        {
            CreateMap<CreateDatacenterRequest, Domain.Models.Datacenter>();
            CreateMap<Domain.Models.Datacenter, DatacenterDto>()
                 //.IncludeBase<ConfigurationEntity, ConfigurationEntityDto>();
                 .ForMember(d => d.StartDate, options =>
                 {
                     options.MapFrom(src => src.StartDate);
                 })
                 .ForMember(d => d.LocationCityCode, options => options.MapFrom(src => src.Location.CityCode))
                 .ForMember(d => d.LocationCountryCode, options => options.MapFrom(src => src.Location.CountryCode))
                 .ForMember(d => d.LocationRegionCode, options => options.MapFrom(src => src.Location.RegionCode));


            CreateMap<Domain.Models.Datacenter, DatacenterIntegrationEvent>()
                .ForMember(d => d.Id, options => options.Ignore());


            CreateMap<Domain.Models.Datacenter, DatacenterPluginsDto>()
                 //.IncludeBase<ConfigurationEntity, ConfigurationEntityDto>();
                 .ForMember(d => d.DatacenterId, options => options.MapFrom(src => src.Id))
                 .ForMember(d => d.PluginName, options => options.MapFrom(src => src.Plugins.Select(p => p.Plugin.Name)))
                 .ForMember(d => d.CredentialName, options => options.MapFrom(src => src.Plugins.Select(p => p.Credential.Name)));

        }
    }
}
