using AutoMapper;
using Inventory.Provisioning.Domain.Models;

namespace Inventory.Provisioning.Api.Applications.LabelNames
{
    public class LabelNameProfile : Profile
    {
        public LabelNameProfile() 
        { 
            CreateMap<LabelName, LabelNameDto>();
        }
    }
}
