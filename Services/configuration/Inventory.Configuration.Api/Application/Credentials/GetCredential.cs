using Inventory.Common.Application.Core;
using MediatR;

namespace Inventory.Configuration.Api.Application.Credentials
{
    public class GetCredentialById : IRequest<Payload<CredentialDto>>
    {
        public int Id { get; set; }
    }

    public class GetCredentialByName : IRequest<Payload<CredentialDto>>
    {
        public string Name { get; set; }
    }

}
