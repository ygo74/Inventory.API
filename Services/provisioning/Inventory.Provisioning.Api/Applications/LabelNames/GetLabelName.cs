using Ardalis.GuardClauses;
using AutoMapper;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Exceptions;
using Inventory.Common.Domain.Filters;
using Inventory.Provisioning.Domain.Filters;
using Inventory.Provisioning.Domain.Models;
using Inventory.Provisioning.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MR.AspNetCore.Pagination;

namespace Inventory.Provisioning.Api.Applications.LabelNames
{
    /// <summary>
    /// Get Label Name by Id
    /// </summary>
    public class GetLabelNameById : IRequest<Payload<LabelNameDto>>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Get Label Name by Name
    /// </summary>
    public class GetLabelNameByName : IRequest<Payload<LabelNameDto>>
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// Get Label Names
    /// </summary>
    public class GetLabelNameRequest : QueryEntityCursorPaginationRequest<LabelNameDto>
    {
        public string Name { get; set; }
    }

    public class GetLabelNameHandler : IRequestHandler<GetLabelNameById, Payload<LabelNameDto>>,
                                       IRequestHandler<GetLabelNameByName, Payload<LabelNameDto>>,
                                       IRequestHandler<GetLabelNameRequest, CursorPaginationdPayload<LabelNameDto>>
    {

        private readonly ILogger<GetLabelNameHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<ProvisioningDbContext> _factory;
        private readonly IPaginationService _paginationService;

        public GetLabelNameHandler(ILogger<GetLabelNameHandler> logger,
            IMapper mapper, IDbContextFactory<ProvisioningDbContext> factory, IPaginationService paginationService)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _factory = Guard.Against.Null(factory, nameof(factory));
            _paginationService = Guard.Against.Null(paginationService, nameof(paginationService));

        }


        /// <summary>
        /// Get Label by ID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Payload<LabelNameDto>> Handle(GetLabelNameById request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start getting Label Name with id : {0}", request.Id);

            await using var dbContext = _factory.CreateDbContext();

            var entity = await dbContext.LabelNames.FindAsync(request.Id);

            if (null == entity)
                return Payload<LabelNameDto>.Error(new NotFoundError($"Don't find Label Name with Id {request.Id}"));

            _logger.LogInformation("Successfully get Label Name with id {0}", request.Id);
            return Payload<LabelNameDto>.Success(_mapper.Map<LabelNameDto>(entity));

        }

        /// <summary>
        /// Get Label By Name
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Payload<LabelNameDto>> Handle(GetLabelNameByName request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Start getting Label Name with name : {0}", request.Name);

            await using var dbContext = _factory.CreateDbContext();

            var query = dbContext.LabelNames.AsQueryable();

            // Filtering data
            var filter = ExpressionFilterFactory.Create<LabelName>()
                            .WithExactName(request.Name);

            if (null != filter.Predicate)
                query = query.Where(filter.Predicate);

            var entity = await query.SingleOrDefaultAsync();

            if (null == entity)
                return Payload<LabelNameDto>.Error(new NotFoundError($"Don't find Label Name with Name {request.Name}"));

            _logger.LogInformation("Successfully get Label Name with Name {0}", request.Name);
            return Payload<LabelNameDto>.Success(_mapper.Map<LabelNameDto>(entity));

        }

        /// <summary>
        /// Get labels according multiple parameters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<CursorPaginationdPayload<LabelNameDto>> Handle(GetLabelNameRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
