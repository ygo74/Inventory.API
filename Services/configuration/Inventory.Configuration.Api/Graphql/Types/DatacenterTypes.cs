using HotChocolate.Types;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Plugins;
using Inventory.Configuration.Api.Application.Datacenters;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Networks.Domain.Models;
using Inventory.Plugins.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.Configuration.Api.Graphql.Types
{
    public class DatacenterType : ObjectType<DatacenterDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DatacenterDto> descriptor)
        {
            descriptor.Name("DatacenterDto");
            descriptor.Field(e => e.Code).Name("Code").Description("Data center unique code");


            // add field to get pluginenpoints with the dataloader PluginEndpointByDatacenterDataloader


            // add hotchocolate fied to get plugin in batch mode with a batchdataloader
            descriptor.Field("plugins")
                .Type<ListType<DatacenterPluginsType>>()
                .Resolve<DatacenterPluginsDto[]>(async (ctx, cancellationToken) =>
                {
                    var datacenter = ctx.Parent<DatacenterDto>();
                    var loader = ctx.GroupDataLoader<int, DatacenterPluginsDto>(
                                               async (keys, ct) =>
                                               {
                                                   var request = new GetPluginsByDatacenterIdRequest
                                                   {
                                                       DatacenterIds = keys
                                                   };

                                                   var result = await ctx.Service<IMediator>().Send(request, ctx.RequestAborted);
                                                   return result.ToLookup(e => e.DatacenterId);
                                               }, "GetPluginsByDatacenterId"
                                               );

                    return await loader.LoadAsync(datacenter.Id);
                });


            descriptor.Field("subnets")
                .Resolve<List<Subnet>>(async (ctx, cancellationToken) =>
                {
                    var datacenter = ctx.Parent<DatacenterDto>();
                    var request = new GetDatacenterSubnetsRequest
                    {
                        DatacenterCode = datacenter.Code
                    };
                    return await ctx.Service<IMediator>().Send(request, cancellationToken);
                });

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

    //public class DatacenterPayloadType : ObjectType<Payload<DatacenterDto>>
    //{
    //    protected override void Configure(IObjectTypeDescriptor<Payload<DatacenterDto>> descriptor)
    //    {
    //        descriptor.Name("DatacenterPayload");
    //        descriptor.Field(e => e.Data).Type<DatacenterType>();
    //    }
    //}

    public class DatacenterPluginsType : ObjectType<DatacenterPluginsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DatacenterPluginsDto> descriptor)
        {
            descriptor.Name("DatacenterPlugins");
            descriptor.Field(e => e.PluginEndpointPropertyBag).Type<AnyType>();
            //descriptor.Field(e => e.CredentialPropertyBag).Type<AnyType>();
            descriptor.Ignore(e => e.CredentialPropertyBag);
        }
    }


}
