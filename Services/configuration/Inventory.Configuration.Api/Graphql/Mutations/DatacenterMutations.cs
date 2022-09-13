using HotChocolate;
using HotChocolate.Types;
using Inventory.Configuration.Api.Application.Datacenter;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Graphql.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class DatacenterMutations
    {
        public async Task<CreateDatacenter.Payload> CreateServer(
            CreateDatacenter.Command command,
            [Service] IMediator _mediator
            )
        {
            return await _mediator.Send(command);
        }

    }
}
