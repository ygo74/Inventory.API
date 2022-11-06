using HotChocolate.Types;
using Inventory.Configuration.Api.Application.Datacenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Graphql.Types
{
    public class DatacenterType : ObjectType<DatacenterDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DatacenterDto> descriptor)
        {
            descriptor.Name("DatacenterDto");
            descriptor.Field(e => e.Code).Name("Code").Description("Data center unique code");
        }
    }

    public class CreateDatacenterInputType : ObjectType<CreateDatacenter.Command>
    {
        protected override void Configure(IObjectTypeDescriptor<CreateDatacenter.Command> descriptor)
        {
            descriptor.Name("CreateDatacenter");
            descriptor.Field(e => e.Code).Name("Code").Description("Data center unique code");
        }
    }

    public class CreateDatacenterPayloadType : ObjectType<CreateDatacenter.Payload>
    {
        protected override void Configure(IObjectTypeDescriptor<CreateDatacenter.Payload> descriptor)
        {
            descriptor.Name("CreateDatacenterPayload");
            descriptor.Field(e => e.Datacenter).Type<DatacenterType>();
        }
    }


}
