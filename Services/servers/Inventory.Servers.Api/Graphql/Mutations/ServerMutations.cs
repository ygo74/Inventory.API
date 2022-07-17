using HotChocolate;
using HotChocolate.Types;
using Inventory.Servers.Api.Applications.Servers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Servers.Api.Graphql.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class ServerMutations
    {


        public async Task<CreateServer.CreateServerPayload> CreateServer(
            CreateServer.Command command,
            [Service] IMediator _mediator
            )
        {
            return await _mediator.Send(command);
        }

    }
}
