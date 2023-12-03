using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Exceptions;
using Inventory.Common.Domain.Filters;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MR.AspNetCore.Pagination;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenters
{
    /// <summary>
    /// Get Datacenter By ID
    /// </summary>
    public class GetDatacenterByIdRequest : QueryEntityByIdRequest<DatacenterDto> { }

    /// <summary>
    /// Get Datacenter By Name
    /// </summary>
    public class GetDatacenterByNameRequest : IRequest<Payload<DatacenterDto>>
    {
        public string Name { get; set; }
    }

#nullable enable
    public class GetDatacenterRequest : QueryConfigurationCursorPaginationRequest<DatacenterDto>
    {
        public string? InventoryCode { get; set; }
    }
#nullable disable


    public class DatacenterQueriesHandler : IRequestHandler<GetDatacenterByIdRequest, Payload<DatacenterDto>>,
                                            IRequestHandler<GetDatacenterByNameRequest, Payload<DatacenterDto>>,
                                            IRequestHandler<GetDatacenterRequest, CursorPaginationdPayload<DatacenterDto>>
    {

        private readonly ILogger<DatacenterQueriesHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;
        private readonly IPaginationService _paginationService;

        public DatacenterQueriesHandler(ILogger<DatacenterQueriesHandler> logger,
            IMapper mapper, IDbContextFactory<ConfigurationDbContext> factory, IPaginationService paginationService)
        {

            _logger = Guard.Against.Null(logger, nameof(logger));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _factory = Guard.Against.Null(factory, nameof(factory));
            _paginationService = Guard.Against.Null(paginationService, nameof(paginationService));

        }

        /// <summary>
        /// Get Datacenter By ID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<DatacenterDto>> Handle(GetDatacenterByIdRequest request, CancellationToken cancellationToken)
        {
            await using var dbContext = _factory.CreateDbContext();

            var datacenter = await dbContext.Datacenters.FindAsync(keyValues: new object[] { request.Id }, 
                                                                   cancellationToken: cancellationToken);

            if (null == datacenter)
                Payload<DatacenterDto>.Error(new NotFoundError($"Don't find Datacenter with Id {request.Id}"));

            return Payload<DatacenterDto>.Success(_mapper.Map<DatacenterDto>(datacenter));
        }

        /// <summary>
        /// Get Datacenter By Name
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<Payload<DatacenterDto>> Handle(GetDatacenterByNameRequest request, CancellationToken cancellationToken)
        {
            await using var dbContext = _factory.CreateDbContext();

            var filter = ExpressionFilterFactory.Create<Inventory.Configuration.Domain.Models.Datacenter>()
                                                .WithName(request.Name);

            var datacenter = await dbContext.Datacenters
                                            .AsNoTracking()
                                            .Where(filter.Predicate)
                                            .ProjectTo<DatacenterDto>(_mapper.ConfigurationProvider)
                                            .FirstOrDefaultAsync(cancellationToken);

            if (null == datacenter)
                Payload<DatacenterDto>.Error(new NotFoundError($"Don't find Datacenter with Name {request.Name}"));

            return Payload<DatacenterDto>.Success(datacenter);

        }

        /// <summary>
        /// Get Datacenters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<CursorPaginationdPayload<DatacenterDto>> Handle(GetDatacenterRequest request, CancellationToken cancellationToken)
        {
            await using var dbContext = _factory.CreateDbContext();
            var query = dbContext.Datacenters.AsNoTracking();

            // Filtering data
            var filter = request.GetConfigurationEntityFilter<Inventory.Configuration.Domain.Models.Datacenter, DatacenterDto>()
                                .WithInventoryCode(request.InventoryCode);  

            if (null != filter.Predicate)
                query = query.Where(filter.Predicate);

            // call keyset pagination
            var result = await _paginationService.KeysetPaginateAsync(
                source: query,
                builderAction: b => b.Ascending(e => e.Name).Ascending(e => e.Id),
                getReferenceAsync: async id => await dbContext.Datacenters.FindAsync(int.Parse(id)),
                map: q => q.ProjectTo<DatacenterDto>(_mapper.ConfigurationProvider),
                queryModel: new KeysetQueryModel
                {
                    After = request.Pagination.After,
                    Before = request.Pagination.Before,
                    Size = request.Pagination.Size
                }
            );

            // return paginated result
            return new CursorPaginationdPayload<DatacenterDto>
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
