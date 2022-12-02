using AutoMapper;
using Inventory.Configuration.Domain.Models;

namespace Inventory.Configuration.Api.Application.Credentials
{
    public class CredentialMappingProfile : Profile
    {
        public CredentialMappingProfile() 
        {
            CreateMap<Credential, CredentialDto>();
        }
    }
}
