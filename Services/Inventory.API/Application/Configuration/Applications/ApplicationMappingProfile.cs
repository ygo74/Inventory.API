using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Application.Configuration.Applications
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<CreateApplication.Command, Inventory.Domain.Models.Configuration.Application>();
            CreateMap<Inventory.Domain.Models.Configuration.Application, ApplicationDto>();
        }
    }
}
