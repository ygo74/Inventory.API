using HotChocolate;
using HotChocolate.Types;
using Inventory.Common.Application.Core;
using Inventory.Configuration.Api.Application.Credentials;
using Inventory.Configuration.Api.Application.Credentials.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Graphql.Mutations
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class CredentialMutations
    {

        /// <summary>
        /// Create Credential
        /// </summary>
        /// <param name="input"></param>
        /// <param name="mediator"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<CredentialDto>> CreateCredential(
            CreateCredentialRequest input,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(input, cancellationToken);
        }

        /// <summary>
        /// Update Credential
        /// </summary>
        /// <param name="input"></param>
        /// <param name="mediator"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<CredentialDto>> UpdateCredential(
            UpdateCredentialRequest input,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(input, cancellationToken);
        }

        /// <summary>
        /// Remove Credential
        /// </summary>
        /// <param name="input"></param>
        /// <param name="mediator"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<CredentialDto>> RemoveCredential(
            RemoveCredentialRequest input,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            return await mediator.Send(input, cancellationToken);
        }
    }
}
