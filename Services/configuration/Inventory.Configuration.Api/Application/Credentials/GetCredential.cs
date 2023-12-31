using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Errors;
using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Credentials.Dtos;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Domain.Filters;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MR.AspNetCore.Pagination;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using System;

namespace Inventory.Configuration.Api.Application.Credentials
{
    /// <summary>
    /// Get credential by Id
    /// </summary>
    public class GetCredentialByIdRequest : IRequest<Payload<CredentialDto>>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Get credential by Name
    /// </summary>
    public class GetCredentialByNameRequest : IRequest<Payload<CredentialDto>>
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// Get credentials
    /// </summary>
    public class GetCredentialsRequest : QueryEntityOffsetPaginationRequest<CredentialDto>
    {
        public string Name { get; set; }
    }


    public class GetCredentialHandler : IRequestHandler<GetCredentialByIdRequest, Payload<CredentialDto>>,
                                        IRequestHandler<GetCredentialByNameRequest, Payload<CredentialDto>>,
                                        IRequestHandler<GetCredentialsRequest, OffsetPaginationPayload<CredentialDto>>
    {

        private readonly ILogger<GetCredentialHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;
        private readonly IGenericQueryStore<Credential> _queryStore;

        public GetCredentialHandler(ILogger<GetCredentialHandler> logger,
            IMapper mapper, IGenericQueryStore<Credential> queryStore, IPaginationService paginationService)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _queryStore = Guard.Against.Null(queryStore, nameof(queryStore));
            _paginationService = Guard.Against.Null(paginationService, nameof(paginationService));

        }

        /// <summary>
        /// Get credential by id handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<CredentialDto>> Handle(GetCredentialByIdRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start getting credential with id : {0}", request.Id);

            // retrieve entity
            var credential = await _queryStore.GetByIdAsync<CredentialDto>(request.Id, CredentialDto.Projection);

            if (null == credential)
            {
                var errorMessage = $"Don't find credential with Id {request.Id}";
                _logger.LogInformation(errorMessage);
                return Payload<CredentialDto>.Error(new NotFoundError(errorMessage));
            }

            _logger.LogInformation("Successfully get credential with id {0}", request.Id);
            return Payload<CredentialDto>.Success(credential);

        }

        /// <summary>
        /// Get Credential by name handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<CredentialDto>> Handle(GetCredentialByNameRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start getting credential with name : {0}", request.Name);

            // Create filter
            var filter = ExpressionFilterFactory.Create<Credential>()
                                                .WithName(request.Name);

            // retrieve entity
            var credential = await _queryStore.FirstOrDefaultAsync<CredentialDto>(filter, CredentialDto.Projection);

            if (null == credential)
            {
                var errorMessage = $"Don't find credential with Name {request.Name}";
                _logger.LogInformation(errorMessage);
                return Payload<CredentialDto>.Error(new NotFoundError(errorMessage));
            }

            // return credential
            _logger.LogInformation("Successfully get credential with name {0}", request.Name);
            return Payload<CredentialDto>.Success(credential);

        }

        public async Task<OffsetPaginationPayload<CredentialDto>> Handle(GetCredentialsRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start searching credential");

            // Filtering data
            var filter = ExpressionFilterFactory.Create<Credential>()
                                                .WithName(request.Name);


            // Count number of total elements
            var countEntities = await _queryStore.CountAsync(filter, cancellationToken);

            // Retrieve entities
            var result = await _queryStore.GetByCriteriaAsync<CredentialDto>(
                criteria: filter,
                orderBy: q => q.OrderBy(e => e.Id),
                Projection: CredentialDto.Projection,
                offset: request.Pagination.Skip,
                limit: request.Pagination.Size                
            );

            // return offset pagination payload
            _logger.LogInformation("End searching credentials");
            var page = (int)Math.Ceiling((double)request.Pagination.Skip / request.Pagination.Size);
            return new OffsetPaginationPayload<CredentialDto>
            {
                TotalCount = countEntities,
                Data = result.ToList(),
                Page = page == 0 ? 1 : page,
                PageSize = request.Pagination.Size,
                PageCount = (int)Math.Ceiling(Decimal.Divide(countEntities, request.Pagination.Size))
            };

        }
    }

}
