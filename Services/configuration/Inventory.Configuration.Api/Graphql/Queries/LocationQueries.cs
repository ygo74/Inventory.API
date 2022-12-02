using HotChocolate.Resolvers;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Inventory.Configuration.Api.Application.Plugin;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using Inventory.Configuration.Api.Application.Locations;
using Inventory.Common.Application.Graphql.Extensions;
using System.Linq;
using Inventory.Common.Application.Core;

namespace Inventory.Configuration.Api.Graphql.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class LocationQueries
    {
        [UsePaging(DefaultPageSize = 10)]
        public async Task<Connection<LocationDto>> GetLocations([Service] IMediator mediator,
            CancellationToken cancellationToken, IResolverContext ctx,
            string? cityCode, string? countryCode, string? regionCode, DateTime? validFrom, DateTime? validTo, bool? includeDeprecated = false, bool? includeAllEntitites = false)
        {

            var request = new GetLocationRequest
            {
                Pagination = ctx.GetCursorPaggingRequest(),
                AllEntities = includeAllEntitites.Value,
                IncludeDeprecated = includeDeprecated.Value,
                CityCode = cityCode,
                CountryCode = countryCode,
                RegionCode = regionCode,
                ValidFrom = validFrom,
                ValidTo = validTo
            };

            var result = await mediator.Send(request, cancellationToken);
            var edges = result.Data.Select(e => new Edge<LocationDto>(e, e.Id.ToString())).ToList();

            var pageInfo = new ConnectionPageInfo(result.HasNext, result.HasPrevious, result.StartCursor, result.EndCursor);

            var connection = new Connection<LocationDto>(edges, pageInfo, result.TotalCount);
            return connection;
        }

        public async Task<Payload<LocationDto>> GetLocation([Service] IMediator mediator,
            CancellationToken cancellationToken, IResolverContext ctx,
            int id)
        {

            var request = new GetLocationByIdRequest
            {
                Id = id
            };

            var result = await mediator.Send(request, cancellationToken);
            return result;
        }

    }
}
