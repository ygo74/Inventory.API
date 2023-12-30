using HotChocolate.Resolvers;
using HotChocolate;
using HotChocolate.Types;
using Inventory.Common.Application.Core;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Inventory.Configuration.Api.Application.Credentials;
using HotChocolate.Types.Pagination;
using Inventory.Common.Application.Graphql.Extensions;
using System.Linq;
using Inventory.Configuration.Api.Application.Credentials.Dtos;

namespace Inventory.Configuration.Api.Graphql.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class CredentialQueries
    {
        /// <summary>
        /// Get Credential by Id
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="ctx"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Payload<CredentialDto>> GetCredential([Service] IMediator mediator,
            CancellationToken cancellationToken, IResolverContext ctx,
            int id)
        {

            var request = new GetCredentialById
            {
                Id = id
            };

            var result = await mediator.Send(request, cancellationToken);
            return result;
        }

        /// <summary>
        /// Get credential by name
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="ctx"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Payload<CredentialDto>> GetCredentialByName([Service] IMediator mediator,
            CancellationToken cancellationToken, IResolverContext ctx,
            string name)
        {

            var request = new GetCredentialByName
            {
                Name = name
            };

            var result = await mediator.Send(request, cancellationToken);
            return result;
        }

        /// <summary>
        /// Get Credentials
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        [UsePaging(DefaultPageSize = 10)]
        public async Task<Connection<CredentialDto>> GetCredentials([Service] IMediator mediator,
            CancellationToken cancellationToken, IResolverContext ctx)
        {
            var request = new GetCredentialsRequest
            {
                Pagination = ctx.GetCursorPaggingRequest()
            };

            var result = await mediator.Send(request, cancellationToken);
            var edges = result.Data.Select(e => new Edge<CredentialDto>(e, e.Id.ToString())).ToList();

            var pageInfo = new ConnectionPageInfo(result.HasNext, result.HasPrevious, result.StartCursor, result.EndCursor);

            var connection = new Connection<CredentialDto>(edges, pageInfo, result.TotalCount);
            return connection;


        }
    }
}
