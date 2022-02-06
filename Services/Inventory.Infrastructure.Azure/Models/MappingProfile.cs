using AutoMapper;
using Inventory.Domain.Models.ManagedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Azure.Models
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
