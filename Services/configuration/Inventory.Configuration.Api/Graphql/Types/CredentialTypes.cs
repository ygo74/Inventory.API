using HotChocolate.Types;
using Inventory.Configuration.Api.Application.Credentials;
using Inventory.Configuration.Api.Application.Credentials.Dtos;

namespace Inventory.Configuration.Api.Graphql.Types
{
    public class CredentialType : ObjectType<CredentialDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CredentialDto> descriptor)
        {
            descriptor.Field(e => e.PropertyBag).Type<AnyType>();
        }
    }

    public class CreateCredentialRequestType : ObjectType<CreateCredentialRequest> 
    {
        protected override void Configure(IObjectTypeDescriptor<CreateCredentialRequest> descriptor)
        {
            //descriptor.Field(e => e.PropertyBag).Type<AnyType>();
            descriptor.Ignore(e => e.PropertyBag);
        }
    }
}
