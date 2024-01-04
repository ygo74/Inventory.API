using Inventory.Common.Application.Dto;
using System.Collections.Generic;
using System;
using Inventory.Configuration.Domain.Models;
using System.Linq.Expressions;

namespace Inventory.Configuration.Api.Application.Credentials.Dtos
{
    public class CredentialDto : AuditEntityDto
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Dictionary<string, object> PropertyBag { get; set; }

        public static Expression<Func<Credential, CredentialDto>> Projection
        {
            get
            {
                return e => new CredentialDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    PropertyBag = e.PropertyBag,
                    CreatedBy = e.CreatedBy,
                    Created = e.Created,
                    LastModifiedBy = e.LastModifiedBy,
                    LastModified = e.LastModified          
                };
            }
        }

    }

    public static class CredentialExtension
    {
        private static Func<Credential, CredentialDto> _credentialToDtoFunc = CredentialDto.Projection.Compile();

        public static CredentialDto ToCredentialDto(this Credential credential)
        {
            return _credentialToDtoFunc(credential);
        }
    }
}
