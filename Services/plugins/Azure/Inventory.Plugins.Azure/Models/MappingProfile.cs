using AutoMapper;
using Inventory.Networks.Domain.Models;

namespace Inventory.Plugins.Azure.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AzVirtualNetwork, Subnet>()
                .ForMember(d => d.CIDR, options =>
                {
                    options.MapFrom(s => s.SubnetCIDR);
                })
                .ForMember(d => d.ProviderId, options => options.MapFrom(s => s.SubnetId))
                .ForMember(d => d.Provider, options =>
                {
                    options.MapFrom(src => "Azure");
                });

        }
    }
}
