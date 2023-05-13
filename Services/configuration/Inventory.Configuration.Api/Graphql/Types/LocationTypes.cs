using HotChocolate.Types;
using Inventory.Configuration.Api.Application.Locations;

namespace Inventory.Configuration.Api.Graphql.Types
{
    public class GetLocationRequestType : ObjectType<GetLocationRequest>
    {
        protected override void Configure(IObjectTypeDescriptor<GetLocationRequest> descriptor)
        {
            descriptor.Name("GetLocationRequest");
            descriptor.Ignore(e => e.Pagination);
        }
    }
}
