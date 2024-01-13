using Ardalis.GuardClauses;
using AutoMapper;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Infrastructure;
using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MR.AspNetCore.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Configuration.Api.Application.Plugins.Dtos;
using Inventory.Common.Application.Errors;
using Inventory.Configuration.Api.Application.Plugins.Services;

namespace Inventory.Configuration.Api.Application.Plugins
{

    /// <summary>
    /// Get plugin request by Id
    /// </summary>
    public class GetPluginByIdRequest : QueryEntityByIdRequest<PluginDto>
    {
    }

    /// <summary>
    /// Get plugin request by Code
    /// </summary>
    public class GetPluginByCodeRequest : IRequest<Payload<PluginDto>>
    {
        public string Code { get; set; }
    }

#nullable enable

    public class GetPluginRequest : QueryConfigurationCursorPaginationRequest<PluginDto>
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Version { get; set; }
    }


#nullable disable

    public class GetPluginHandler : IRequestHandler<GetPluginRequest, CursorPaginationdPayload<PluginDto>>,
                                    IRequestHandler<GetPluginByIdRequest, Payload<PluginDto>>,
                                    IRequestHandler<GetPluginByCodeRequest, Payload<PluginDto>>
    {
        private readonly IGenericQueryStore<Domain.Models.Plugin> _queryStore;
        private readonly ILogger<GetPluginHandler> _logger;
        private readonly PluginService _pluginService;
        private readonly IPaginationService _paginationService;

        public GetPluginHandler(IGenericQueryStore<Domain.Models.Plugin> queryStore, ILogger<GetPluginHandler> logger,
                                PluginService pluginService, IPaginationService paginationService)
        {
            _queryStore = Guard.Against.Null(queryStore, nameof(queryStore));
            _logger = Guard.Against.Null(logger, nameof(logger));
            _pluginService = Guard.Against.Null(pluginService, nameof(pluginService));
            _paginationService = Guard.Against.Null(paginationService,nameof(paginationService));
        }




        /// <summary>
        /// Get plugins
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CursorPaginationdPayload<PluginDto>> Handle(GetPluginRequest request, CancellationToken cancellationToken)
        {

            // Filtering data
            var filter = request.GetConfigurationEntityFilter<Domain.Models.Plugin, PluginDto>()
                                    .WithName(request.Name)
                                    .WithCode(request.Code);


            var result = await _paginationService.KeysetPaginateAsync(
                source: _queryStore.GetQuery(filter),
                builderAction: b => b.Ascending(e => e.Code).Ascending(e => e.Id),
                getReferenceAsync: async id => await _queryStore.GetByIdAsync(int.Parse(id)),
                map: q => q.Select(e => _pluginService.GetPluginDto(e)),
                queryModel: new KeysetQueryModel
                {
                    After = request.Pagination.After,
                    Before = request.Pagination.Before,
                    Size = request.Pagination.Size
                }
                );

            return new CursorPaginationdPayload<PluginDto>
            {
                TotalCount = result.TotalCount,
                Data = result.Data,
                HasNext = result.HasNext,
                HasPrevious = result.HasPrevious,
                StartCursor = result.Data.Count > 0 ? result.Data[0].Id.ToString() : string.Empty,
                EndCursor = result.Data.Count > 0 ? result.Data[result.Data.Count - 1].Id.ToString() : string.Empty,
            };

        }

        /// <summary>
        /// Get plugin by Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Payload<PluginDto>> Handle(GetPluginByIdRequest request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Start retrieving plugin with id '{0}'", request.Id);

            // Get entity
            var entity = await _queryStore.GetByIdAsync(request.Id);

            if (entity is null)
            {
                var errorMessage = $"Error when retrieving plugin with id '{request.Id}'";
                return Payload<PluginDto>.Error(new NotFoundError(errorMessage));
            }

            // return result
            return Payload<PluginDto>.Success(_pluginService.GetPluginDto(entity));

        }

        /// <summary>
        /// Get plugin by code
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Payload<PluginDto>> Handle(GetPluginByCodeRequest request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Start retrieving plugin with code '{0}'", request.Code);

            // create filter
            var filter = ExpressionFilterFactory.Create<Domain.Models.Plugin>()
                                                .WithCode(request.Code);

            // retrieve entity
            var entity = await _queryStore.FirstOrDefaultAsync(filter);
            if (entity is null)
            {
                var errorMessage = $"Error when retrieving plugin with code '{request.Code}'";
                return Payload<PluginDto>.Error(new NotFoundError(errorMessage));
            }

            // return result
            return Payload<PluginDto>.Success(_pluginService.GetPluginDto(entity));

        }
    }
}
