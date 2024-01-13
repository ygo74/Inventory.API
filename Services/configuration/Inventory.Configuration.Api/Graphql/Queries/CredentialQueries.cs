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

            var request = new GetCredentialByIdRequest
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

            var request = new GetCredentialByNameRequest
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
        [UseOffsetPaging(DefaultPageSize = 2)]
        public async Task<CollectionSegment<CredentialDto>> GetCredentials(
            GetCredentialsRequest request,
            [Service] IMediator mediator,
            CancellationToken cancellationToken, IResolverContext ctx)
        {
            if (request == null) { request = new GetCredentialsRequest(); }
            request.Pagination = ctx.GetOffsetPagingRequest();
            
            var result = await mediator.Send(request, cancellationToken);
            
            var pageInfo = new CollectionSegmentInfo(result.Page < result.PageCount, result.Page > 1);

            var collectionSegment = new CollectionSegment<CredentialDto>(result.Data, pageInfo, ct => ValueTask.FromResult(result.TotalCount));
            return collectionSegment;

        }
    }
}
