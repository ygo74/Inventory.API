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

namespace Inventory.Configuration.Api.Application.Plugin
{
#nullable enable

    public class GetPluginRequest : QueryConfigurationCursorPaginationRequest<PluginDto>
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Version { get; set; }
    }


#nullable disable

    public class GetPluginHandler : IRequestHandler<GetPluginRequest, CursorPaginationdPayload<PluginDto>>
    {
        private readonly IAsyncRepositoryWithSpecification<Domain.Models.Plugin> _repository;
        private readonly ILogger<GetPluginHandler> _logger;
        private readonly IMapper _mapper;
        private readonly PluginService _pluginService;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;
        private readonly IPaginationService _paginationService;

        public GetPluginHandler(IAsyncRepositoryWithSpecification<Domain.Models.Plugin> repository, ILogger<GetPluginHandler> logger,
            IMapper mapper, PluginService pluginService, IDbContextFactory<ConfigurationDbContext> factory, IPaginationService paginationService)
        {
            _repository = Guard.Against.Null(repository, nameof(repository));
            _logger = Guard.Against.Null(logger, nameof(logger));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _pluginService = Guard.Against.Null(pluginService, nameof(pluginService));
            _factory = Guard.Against.Null(factory, nameof(factory));
            _paginationService = Guard.Against.Null(paginationService,nameof(paginationService));
        }


        public async Task<CursorPaginationdPayload<PluginDto>> Handle(GetPluginRequest request, CancellationToken cancellationToken)
        {
            await using var dbContext = _factory.CreateDbContext();

            var query = dbContext.Plugins.AsQueryable();

            // Filtering data
            var filter = request.GetConfigurationEntityFilter<Domain.Models.Plugin, PluginDto>();
            if (!string.IsNullOrWhiteSpace(request.Name)) { filter = filter.WithName(request.Name); }
            if (!string.IsNullOrWhiteSpace(request.Code)) { filter = filter.WithCode(request.Code); }

            if (null != filter.Predicate)
                query = query.Where(filter.Predicate);


            var result = await _paginationService.KeysetPaginateAsync(
                source: query,
                builderAction: b => b.Ascending(e => e.Code).Ascending(e => e.Id),
                getReferenceAsync: async id => await dbContext.Plugins.FindAsync(int.Parse(id)),
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
    }
}
