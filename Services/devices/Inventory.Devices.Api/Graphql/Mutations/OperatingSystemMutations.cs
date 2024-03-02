using HotChocolate;
using HotChocolate.Types;
using Inventory.Common.Application.Core;
using Inventory.Devices.Api.Applications.OperatingSystem;
using Inventory.Devices.Api.Applications.OperatingSystem.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Devices.Api.Graphql.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class OperatingSystemMutations
    {

        public async Task<Payload<OperatingSystemDto>> CreateOperatingSystem(
            CreateOperatingSystemRequest command,
            [Service] IMediator _mediator
            )
        {
            return await _mediator.Send(command);
        }

    }
}
