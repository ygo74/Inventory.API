using Inventory.Common.Application.Dto;

namespace Inventory.Configuration.Api.Application.Credentials
{
    public class CredentialDto : AuditEntityDto
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}
