using HotChocolate.Types;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Api.Application.Datacenters;
using Inventory.Configuration.Api.Application.Locations;
using Inventory.Configuration.Api.Application.Locations.Dtos;
using MediatR;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

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

    public class LocationType : ObjectType<LocationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<LocationDto> descriptor)
        {
            // add hotchocolate fied to get plugin in batch mode with a batchdataloader
            descriptor.Field("datacenters")
                .Type<ListType<DatacenterType>>()
                .Resolve<DatacenterDto[]>(async (ctx, cancellationToken) =>
                {
                    var location = ctx.Parent<LocationDto>();
                    var loader = ctx.GroupDataLoader<int, DatacenterDto>(
                                               async (keys, ct) =>
                                               {
                                                   var request = new GetDatacenterByLocationIdsRequest
                                                   {
                                                       LocationIds = keys
                                                   };

                                                   var result = await ctx.Service<IMediator>().Send(request, ctx.RequestAborted);
                                                   return result.ToLookup(e => e.LocationId);
                                               }, "GetDatacentersByLocationId"
                                               );

                    return await loader.LoadAsync(location.Id);
                });

        }
    }
}
