using Inventory.Common.Application.Dto;
using System.Collections.Generic;
using System;

namespace Inventory.Configuration.Api.Application.Credentials
{
    public class CredentialDto : AuditEntityDto
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Dictionary<string, object> PropertyBag { get; set; }

    }
}
