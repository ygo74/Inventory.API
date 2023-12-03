using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Errors;
using Inventory.Common.Domain.Filters;
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
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;
        private readonly IPaginationService _paginationService;

        public LocationQueriesHandler(ILogger<LocationQueriesHandler> logger,
            IMapper mapper, IDbContextFactory<ConfigurationDbContext> factory, IPaginationService paginationService)
        {

            _logger = Guard.Against.Null(logger, nameof(logger));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _factory = Guard.Against.Null(factory, nameof(factory));
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
            await using var dbContext = _factory.CreateDbContext();

            var location = await dbContext.Locations.FindAsync(request.Id);

            if (null == location)
                return Payload<LocationDto>.Error(new NotFoundError($"Don't find Location with Id {request.Id}"));

            return Payload<LocationDto>.Success(_mapper.Map<LocationDto>(location));

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
            await using var dbContext = _factory.CreateDbContext();

            var filter = ExpressionFilterFactory.Create<Location>();
            filter = filter.WithName(request.Name);
            var location = await dbContext.Locations.FirstOrDefaultAsync(filter.Predicate);

            if (null == location)
                return Payload<LocationDto>.Error(new NotFoundError($"Don't find Location with Name {request.Name}"));

            return Payload<LocationDto>.Success(_mapper.Map<LocationDto>(location));

        }

        /// <summary>
        /// Get Locations
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CursorPaginationdPayload<LocationDto>> Handle(GetLocationRequest request, CancellationToken cancellationToken)
        {
            await using var dbContext = _factory.CreateDbContext();
            var query = dbContext.Locations.AsQueryable().AsNoTracking();

            // Filtering data
            var filter = request.GetConfigurationEntityFilter<Location, LocationDto>();
            if (!string.IsNullOrWhiteSpace(request.CityCode))    { filter = filter.WithCityCode(request.CityCode); }
            if (!string.IsNullOrWhiteSpace(request.CountryCode)) { filter = filter.WithCountryCode(request.CountryCode); }
            if (!string.IsNullOrWhiteSpace(request.RegionCode))  { filter = filter.WithRegionCode(request.RegionCode); }

            if (null != filter.Predicate)
                query = query.Where(filter.Predicate);


            var result = await _paginationService.KeysetPaginateAsync(
                source: query,
                builderAction: b => b.Ascending(e => e.RegionCode).Ascending(e => e.CountryCode).Ascending(e => e.CityCode).Ascending(e => e.Id),
                getReferenceAsync: async id => await dbContext.Locations.FindAsync(int.Parse(id)),
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
