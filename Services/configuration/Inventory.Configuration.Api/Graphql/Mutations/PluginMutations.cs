using HotChocolate;
using HotChocolate.Types;
using Inventory.Api.Base.Core;
using Inventory.Configuration.Api.Application.Plugin;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Graphql.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class PluginMutations
    {

        public async Task<CreatePlugin.Payload> CreatePlugin(
            CreatePlugin.Command input,
            [Service] IMediator _mediator,
            CancellationToken cancellationToken
            )
        {
            return await _mediator.Send(input,cancellationToken);
        }

        //public async Task<CreatePlugin.Payload> CreatePlugin(
        //    string name, string code, string version, bool? deprecated, DateTime? validFrom, DateTime? validTo,
        //    [Service] IMediator _mediator
        //    )
        //{
        //    return await _mediator.Send(new CreatePlugin.Command()
        //    {
        //        Code = code,
        //        Name = name,
        //        Version = version,
        //        Deprecated = deprecated,
        //        ValidFrom = validFrom,
        //        ValidTo = validTo
        //    });
        //}

    }

}
