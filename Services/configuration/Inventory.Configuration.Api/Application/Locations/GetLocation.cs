using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Errors;
using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Api.Application.Locations.Dtos;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MR.AspNetCore.Pagination;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Locations
{
#nullable enable
    public class GetLocationRequest : QueryConfigurationCursorPaginationRequest<LocationDto>
    {
        public string? CountryCode { get; set; }
        public string? CityCode { get; set; }
        public string? RegionCode { get; set; }
    }
#nullable disable

    public class GetLocationByIdRequest : QueryEntityByIdRequest<LocationDto> { }

    public class GetLocationByNameRequest : IRequest<Payload<LocationDto>> 
    { 
        public string Name { get; set; }
    }

    public class LocationQueriesHandler : IRequestHandler<GetLocationRequest, CursorPaginationdPayload<LocationDto>>,
                                          IRequestHandler<GetLocationByIdRequest, Payload<LocationDto>>,
                                          IRequestHandler<GetLocationByNameRequest, Payload<LocationDto>>
    {

        private readonly ILogger<LocationQueriesHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IGenericQueryStore<Location> _queryStore;
        private readonly IPaginationService _paginationService;

        public LocationQueriesHandler(ILogger<LocationQueriesHandler> logger,
            IMapper mapper, IGenericQueryStore<Location> queryStore, IPaginationService paginationService)
        {

            _logger = Guard.Against.Null(logger, nameof(logger));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _queryStore = Guard.Against.Null(queryStore, nameof(queryStore));
            _paginationService = Guard.Against.Null(paginationService, nameof(paginationService));

        }

        /// <summary>
        /// Get Location By ID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<Payload<LocationDto>> Handle(GetLocationByIdRequest request, CancellationToken cancellationToken)
        {
            // Retrieve entity
            var location = await _queryStore.GetByIdAsync<LocationDto>(request.Id);

            if (null == location)
                return Payload<LocationDto>.Error(new NotFoundError($"Don't find Location with Id {request.Id}"));

            return Payload<LocationDto>.Success(location);

        }


        /// <summary>
        /// Get Location By Name
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<Payload<LocationDto>> Handle(GetLocationByNameRequest request, CancellationToken cancellationToken)
        {
            // Create filter
            var filter = ExpressionFilterFactory.Create<Location>()
                                                .WithName(request.Name);   

            // Retrieve entity
            var location = await _queryStore.FirstOrDefaultAsync<LocationDto>(filter, LocationDto.Projection);

            if (null == location)
                return Payload<LocationDto>.Error(new NotFoundError($"Don't find Location with Name {request.Name}"));

            return Payload<LocationDto>.Success(location);

        }

        /// <summary>
        /// Get Locations
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CursorPaginationdPayload<LocationDto>> Handle(GetLocationRequest request, CancellationToken cancellationToken)
        {
            // Filtering data
            var filter = request.GetConfigurationEntityFilter<Location, LocationDto>()
                            .WithCityCode(request.CityCode)
                            .WithCountryCode(request.CountryCode)
                            .WithRegionCode(request.RegionCode); 


            var result = await _paginationService.KeysetPaginateAsync(
                source: _queryStore.GetQuery(filter),
                builderAction: b => b.Ascending(e => e.RegionCode).Ascending(e => e.CountryCode).Ascending(e => e.CityCode).Ascending(e => e.Id),
                getReferenceAsync: async id => await _queryStore.GetByIdAsync(int.Parse(id)),
                map: q => q.ProjectTo<LocationDto>(_mapper.ConfigurationProvider),
                queryModel: new KeysetQueryModel
                {
                    After = request.Pagination.After,
                    Before = request.Pagination.Before,
                    Size = request.Pagination.Size
                }
                );

            return new CursorPaginationdPayload<LocationDto>
            {
                TotalCount = result.TotalCount,
                Data = result.Data,
                HasNext = result.HasNext,
                HasPrevious = result.HasPrevious,
                StartCursor = result.Data.Count > 0 ? result.Data[0].Id.ToString() : string.Empty,
                EndCursor = result.Data.Count > 0 ? result.Data[result.Data.Count - 1].Id.ToString() : string.Empty,
            };

        }
    }

}
