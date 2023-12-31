using HotChocolate;
using HotChocolate.Types;
using Inventory.Common.Application.Core;
using Inventory.Configuration.Api.Application.Datacenters;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Graphql.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class DatacenterMutations
    {
        /// <summary>
        /// Create Datacenter
        /// </summary>
        /// <param name="input"></param>
        /// <param name="_mediator"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<DatacenterDto>> CreateDatacenter(
            CreateDatacenterRequest input,
            [Service] IMediator _mediator,
            CancellationToken cancellationToken
            )
        {
            return await _mediator.Send(input, cancellationToken);
        }

        /// <summary>
        /// Update Datacenter
        /// </summary>
        /// <param name="input"></param>
        /// <param name="_mediator"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<DatacenterDto>> UpdateDatacenter(
            UpdateDatacenterRequest input,
            [Service] IMediator _mediator,
            CancellationToken cancellationToken
            )
        {
            return await _mediator.Send(input, cancellationToken);
        }


    }
}
