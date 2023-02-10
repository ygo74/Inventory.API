using HotChocolate.Types;
using Inventory.Common.Application.Core;
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

    public class CreateDatacenterInputType : ObjectType<CreateDatacenterRequest>
    {
        protected override void Configure(IObjectTypeDescriptor<CreateDatacenterRequest> descriptor)
        {
            descriptor.Name("CreateDatacenter");
            descriptor.Field(e => e.Code).Name("Code").Description("Data center unique code");
        }
    }

    public class CreateDatacenterPayloadType : ObjectType<Payload<DatacenterDto>>
    {
        protected override void Configure(IObjectTypeDescriptor<Payload<DatacenterDto>> descriptor)
        {
            descriptor.Name("CreateDatacenterPayload");
            descriptor.Field(e => e.Data).Type<DatacenterType>();
        }
    }


}
