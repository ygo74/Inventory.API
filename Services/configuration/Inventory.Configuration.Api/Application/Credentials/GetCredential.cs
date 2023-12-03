using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Errors;
using Inventory.Common.Domain.Filters;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MR.AspNetCore.Pagination;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Credentials
{
    /// <summary>
    /// Get credential by Id
    /// </summary>
    public class GetCredentialById : IRequest<Payload<CredentialDto>>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Get credential by Name
    /// </summary>
    public class GetCredentialByName : IRequest<Payload<CredentialDto>>
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// Get credentials
    /// </summary>
    public class GetCredentialsRequest : QueryEntityCursorPaginationRequest<CredentialDto>
    {
        public string Name { get; set; }
    }


    public class GetCredentialHandler : IRequestHandler<GetCredentialById, Payload<CredentialDto>>,
                                        IRequestHandler<GetCredentialByName, Payload<CredentialDto>>,
                                        IRequestHandler<GetCredentialsRequest, CursorPaginationdPayload<CredentialDto>>
    {

        private readonly ILogger<GetCredentialHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;
        private readonly IPaginationService _paginationService;

        public GetCredentialHandler(ILogger<GetCredentialHandler> logger,
            IMapper mapper, IDbContextFactory<ConfigurationDbContext> factory, IPaginationService paginationService)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _factory = Guard.Against.Null(factory, nameof(factory));
            _paginationService = Guard.Against.Null(paginationService, nameof(paginationService));

        }

        /// <summary>
        /// Get credential by id handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<CredentialDto>> Handle(GetCredentialById request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start getting credential with id : {0}", request.Id);
            bool success = false;
            try
            {
                await using var dbContext = _factory.CreateDbContext();

                var credential = await dbContext.Credentials.FindAsync(request.Id);

                if (null == credential)
                    Payload<CredentialDto>.Error(new NotFoundError($"Don't find credential with Id {request.Id}"));

                return Payload<CredentialDto>.Success(_mapper.Map<CredentialDto>(credential));
            }
            finally
            {
                if (success)
                    _logger.LogInformation("Successfully get credential with id {0}", request.Id);
                else
                    _logger.LogWarning("Unable to get credential with id {0}", request.Id);
            }
        }

        /// <summary>
        /// Get Credential by name handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<CredentialDto>> Handle(GetCredentialByName request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start getting credential with name : {0}", request.Name);
            bool success = false;
            try
            {
                await using var dbContext = _factory.CreateDbContext();

                var credential = await dbContext.Credentials.FirstOrDefaultAsync(e => e.Name == request.Name);

                if (null == credential)
                    Payload<CredentialDto>.Error(new NotFoundError($"Don't find credential with Name {request.Name}"));

                return Payload<CredentialDto>.Success(_mapper.Map<CredentialDto>(credential));
            }
            finally
            {
                if (success)
                    _logger.LogInformation("Successfully get credential with Name {0}", request.Name);
                else
                    _logger.LogWarning("Unable to get credential with Name {0}", request.Name);
            }
        }

        public async Task<CursorPaginationdPayload<CredentialDto>> Handle(GetCredentialsRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start getting credential");
            bool success = false;
            try
            {
                await using var dbContext = _factory.CreateDbContext();

                var query = dbContext.Credentials.AsQueryable();

                // Filtering data
                var filter = ExpressionFilterFactory.Create<Credential>();

                if (null != filter.Predicate)
                    query = query.Where(filter.Predicate);

                var result = await _paginationService.KeysetPaginateAsync(
                    source: query,
                    builderAction: b => b.Ascending(e => e.Id),
                    getReferenceAsync: async id => await dbContext.Credentials.FindAsync(int.Parse(id)),
                    map: q => q.ProjectTo<CredentialDto>(_mapper.ConfigurationProvider),
                    queryModel: new KeysetQueryModel
                    {
                        After = request.Pagination.After,
                        Before = request.Pagination.Before,
                        Size = request.Pagination.Size
                    }
                );

                success = true;
                return new CursorPaginationdPayload<CredentialDto>
                {
                    TotalCount = result.TotalCount,
                    Data = result.Data,
                    HasNext = result.HasNext,
                    HasPrevious = result.HasPrevious,
                    StartCursor = result.Data.Count > 0 ? result.Data[0].Id.ToString() : string.Empty,
                    EndCursor = result.Data.Count > 0 ? result.Data[result.Data.Count - 1].Id.ToString() : string.Empty,
                };

            }
            finally
            {
                if (success)
                    _logger.LogInformation("Successfully get credentials");
                else
                    _logger.LogWarning("Unable to get credentials");
            }
        }
    }

}
