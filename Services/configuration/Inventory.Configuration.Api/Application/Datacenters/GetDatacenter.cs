using Ardalis.GuardClauses;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Errors;
using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Domain.Models;
using MediatR;
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

    /// <summary>
    /// Get Datacenter By Code
    /// </summary>
    public class GetDatacenterByCodeRequest : IRequest<Payload<DatacenterDto>>
    {
        public string Code { get; set; }
    }


#nullable enable
    public class GetDatacenterRequest : QueryConfigurationCursorPaginationRequest<DatacenterDto>
    {
        public string? InventoryCode { get; set; }
    }
#nullable disable


    public class DatacenterQueriesHandler : IRequestHandler<GetDatacenterByIdRequest, Payload<DatacenterDto>>,
                                            IRequestHandler<GetDatacenterByNameRequest, Payload<DatacenterDto>>,
                                            IRequestHandler<GetDatacenterByCodeRequest, Payload<DatacenterDto>>,
                                            IRequestHandler<GetDatacenterRequest, CursorPaginationdPayload<DatacenterDto>>
    {

        private readonly ILogger<DatacenterQueriesHandler> _logger;
        private readonly IPaginationService _paginationService;
        private readonly IGenericQueryStore<Datacenter> _queryStore;

        public DatacenterQueriesHandler(ILogger<DatacenterQueriesHandler> logger,
            IPaginationService paginationService,
            IGenericQueryStore<Datacenter> queryStore)
        {

            _logger = Guard.Against.Null(logger, nameof(logger));
            _paginationService = Guard.Against.Null(paginationService, nameof(paginationService));
            _queryStore = Guard.Against.Null(queryStore, nameof(queryStore));

        }

        /// <summary>
        /// Get Datacenter By ID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<DatacenterDto>> Handle(GetDatacenterByIdRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Get Datacenter by Id {0}", request.Id);

            // Retrieve entity
            var datacenter = await _queryStore.GetByIdAsync<DatacenterDto>(request.Id, DatacenterDto.Projection);

            if (null == datacenter)
            {
                var errorMessage = $"Don't find Datacenter with Id {request.Id}";
                _logger.LogInformation(errorMessage);
                return Payload<DatacenterDto>.Error(new NotFoundError(errorMessage));
            }

            // return datacenter
            _logger.LogInformation("Successfully Get Datacenter by Id {0}", request.Id);
            return Payload<DatacenterDto>.Success(datacenter);
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
            
            _logger.LogInformation("Start Get Datacenter by Name {0}", request.Name);

            // Create filter
            var filter = ExpressionFilterFactory.Create<Datacenter>()
                                                .WithName(request.Name);

            // Retrieve entity
            var datacenter = await _queryStore.FirstOrDefaultAsync<DatacenterDto>(filter, DatacenterDto.Projection);

            if (null == datacenter)
            {
                var errorMessage = $"Don't find Datacenter with Name {request.Name}";
                _logger.LogInformation(errorMessage);
                return Payload<DatacenterDto>.Error(new NotFoundError(errorMessage));
            }

            //return datacenter
            _logger.LogInformation("Successfully Get Datacenter by Name {0}", request.Name);
            return Payload<DatacenterDto>.Success(datacenter);

        }

        /// <summary>
        /// Get datacenter by code
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<Payload<DatacenterDto>> Handle(GetDatacenterByCodeRequest request, CancellationToken cancellationToken)
        {
            // Create filter
            var filter = ExpressionFilterFactory.Create<Datacenter>()
                                                .WithCode(request.Code);

            // Retrieve entity
            var datacenter = await _queryStore.FirstOrDefaultAsync<DatacenterDto>(filter);

            if (null == datacenter)
            {
                var errorMessage = $"Don't find Datacenter with Code {request.Code}";
                _logger.LogInformation(errorMessage);
                return Payload<DatacenterDto>.Error(new NotFoundError(errorMessage));
            }

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
            //await using var dbContext = _factory.CreateDbContext();
            //var query = dbContext.Datacenters.AsNoTracking();

            // Filtering data
            var filter = request.GetConfigurationEntityFilter<Datacenter, DatacenterDto>()
                                .WithInventoryCode(request.InventoryCode);  

            //if (null != filter.Predicate)
            //    query = query.Where(filter.Predicate);

            // call keyset pagination
            var result = await _paginationService.KeysetPaginateAsync(
                source: _queryStore.GetQuery(filter),
                builderAction: b => b.Ascending(e => e.Name).Ascending(e => e.Id),
                getReferenceAsync: async id => await _queryStore.GetByIdAsync(int.Parse(id)),
                map: q => q.Select(DatacenterDto.Projection),
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
