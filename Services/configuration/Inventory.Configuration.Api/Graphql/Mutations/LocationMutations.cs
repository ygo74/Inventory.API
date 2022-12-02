using HotChocolate;
using HotChocolate.Types;
using Inventory.Common.Application.Core;
using Inventory.Configuration.Api.Application.Locations;
using Inventory.Configuration.Api.Application.Plugin;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Inventory.Configuration.Api.Application.Plugin.CreatePlugin;

namespace Inventory.Configuration.Api.Graphql.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class LocationMutations
    {

        public async Task<Payload<LocationDto>> CreateLocation(
            CreateLocationRequest input,
            [Service] IMediator _mediator,
            CancellationToken cancellationToken
            )
        {
            return await _mediator.Send(input,cancellationToken);
        }

        public async Task<Payload<LocationDto>> UpdateLocation(
            UpdateLocationRequest input,
            [Service] IMediator _mediator,
            CancellationToken cancellationToken
            )
        {
            return await _mediator.Send(input, cancellationToken);
        }

        public async Task<Payload<LocationDto>> RemoveLocation(
            DeleteLocationRequest input,            
            [Service] IMediator _mediator,
            CancellationToken cancellationToken
            )
        {
            return await _mediator.Send(input, cancellationToken);
        }

    }

}
