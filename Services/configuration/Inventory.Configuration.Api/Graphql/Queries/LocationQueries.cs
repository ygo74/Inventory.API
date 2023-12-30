using HotChocolate.Resolvers;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Inventory.Configuration.Api.Application.Locations;
using Inventory.Common.Application.Graphql.Extensions;
using System.Linq;
using Inventory.Common.Application.Core;
using Inventory.Configuration.Api.Application.Locations.Dtos;

namespace Inventory.Configuration.Api.Graphql.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class LocationQueries
    {
//#nullable enable
        //[UsePaging(DefaultPageSize = 10)]
        //public async Task<Connection<LocationDto>> GetLocations([Service] IMediator mediator,
        //    CancellationToken cancellationToken, IResolverContext ctx,
        //    string? cityCode, string? countryCode, string? regionCode, DateTime? validFrom, DateTime? validTo, bool? includeDeprecated = false, bool? includeAllEntitites = false)
        //{

        //    var request = new GetLocationRequest
        //    {
        //        Pagination = ctx.GetCursorPaggingRequest(),
        //        AllEntities = includeAllEntitites.HasValue ? includeAllEntitites.Value : false,
        //        IncludeDeprecated = includeDeprecated.HasValue ? includeDeprecated.Value : false,
        //        CityCode = cityCode,
        //        CountryCode = countryCode,
        //        RegionCode = regionCode,
        //        ValidFrom = validFrom,
        //        ValidTo = validTo
        //    };

        //    var result = await mediator.Send(request, cancellationToken);
        //    var edges = result.Data.Select(e => new Edge<LocationDto>(e, e.Id.ToString())).ToList();

        //    var pageInfo = new ConnectionPageInfo(result.HasNext, result.HasPrevious, result.StartCursor, result.EndCursor);

        //    var connection = new Connection<LocationDto>(edges, pageInfo, result.TotalCount);
        //    return connection;
        //}

        [UsePaging(DefaultPageSize = 100)]
        public async Task<Connection<LocationDto>> GetLocations([Service] IMediator mediator,
            CancellationToken cancellationToken, IResolverContext ctx,
            GetLocationRequest request = null)
        {

            if (request == null) { request= new GetLocationRequest(); }
            request.Pagination = ctx.GetCursorPaggingRequest();

            var result = await mediator.Send(request, cancellationToken);
            var edges = result.Data.Select(e => new Edge<LocationDto>(e, e.Id.ToString())).ToList();

            var pageInfo = new ConnectionPageInfo(result.HasNext, result.HasPrevious, result.StartCursor, result.EndCursor);

            var connection = new Connection<LocationDto>(edges, pageInfo, result.TotalCount);
            return connection;
        }

//# nullable disable

        /// <summary>
        /// Get location by ID
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="ctx"></param>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get location by Name
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="ctx"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Payload<LocationWithDatacentersDto>> GetLocationByName([Service] IMediator mediator,
            CancellationToken cancellationToken, IResolverContext ctx,
            string name)
        {

            var request = new GetLocationByNameRequest
            {
                Name = name
            };

            var result = await mediator.Send(request, cancellationToken);
            return result;
        }

    }
}
