﻿using Ardalis.GuardClauses;
using AutoMapper;
using Inventory.Api.Base.Core;
using Inventory.Api.Base.Dto;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Domain.Specifications.PluginSpecifications;
using Inventory.Configuration.Infrastructure;
using Inventory.Domain.Base.Filters;
using Inventory.Domain.Base.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MR.AspNetCore.Pagination;
using MR.EntityFrameworkCore.KeysetPagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Plugin
{
#nullable enable
    public class GetPluginRequest : ConfigurationEntityRequest<QueryCursorPaginatedPayload<PluginDto>>
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Version { get; set; }
    }
#nullable disable

    public class GetPluginHandler : IRequestHandler<GetPluginRequest, QueryCursorPaginatedPayload<PluginDto>>
    {
        private readonly IAsyncRepository<Domain.Models.Plugin> _repository;
        private readonly ILogger<GetPluginHandler> _logger;
        private readonly IMapper _mapper;
        private readonly PluginService _pluginService;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;
        private readonly IPaginationService _paginationService;

        public GetPluginHandler(IAsyncRepository<Domain.Models.Plugin> repository, ILogger<GetPluginHandler> logger,
            IMapper mapper, PluginService pluginService, IDbContextFactory<ConfigurationDbContext> factory, IPaginationService paginationService)
        {
            _repository = Guard.Against.Null(repository, nameof(repository));
            _logger = Guard.Against.Null(logger, nameof(logger));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _pluginService = Guard.Against.Null(pluginService, nameof(pluginService));
            _factory = Guard.Against.Null(factory, nameof(factory));
            _paginationService = Guard.Against.Null(paginationService,nameof(paginationService));
        }

        //public async Task<QueryBasePayload<List<PluginDto>>> Handle(GetPluginRequest request, CancellationToken cancellationToken)
        //{

        //    await using var dbContext = _factory.CreateDbContext();

        //    var query = dbContext.Plugins.AsQueryable();

        //    // Filtering data
        //    var filter = ExpressionFilterFactory.Create<Domain.Models.Plugin>();
        //    if (!string.IsNullOrWhiteSpace(request.Name)) { filter = filter.WithName(request.Name); }
        //    if (!string.IsNullOrWhiteSpace(request.Code)) { filter = filter.WithCode(request.Code); }

        //    if (null != filter.Predicate)
        //        query = query.Where(filter.Predicate);


        //    // Total count
        //    var count = await query.CountAsync();

        //    //keyset pagination
        //    var keysetBuilderAction = (KeysetPaginationBuilder<Domain.Models.Plugin> b) =>
        //    {
        //        b.Ascending(x => x.Code).Ascending(x => x.Id);
        //    };

        //    var data = new List<PluginDto>();
        //    KeysetPaginationContext<Domain.Models.Plugin> keysetContext;

        //    if (request.After != null)
        //    {
        //        var reference = await dbContext.Plugins.FirstOrDefaultAsync(e => e.Id == int.Parse(request.After));
        //        keysetContext = query.KeysetPaginate(keysetBuilderAction, KeysetPaginationDirection.Forward, reference);
        //    }
        //    else if (request.Before != null)
        //    {
        //        var reference = await dbContext.Plugins.FirstOrDefaultAsync(e => e.Id == int.Parse(request.Before));
        //        var direction = reference != null ? KeysetPaginationDirection.Backward : KeysetPaginationDirection.Forward;
        //        keysetContext = query.KeysetPaginate(keysetBuilderAction, direction, reference);
        //    }
        //    else
        //    {
        //        // First page
        //        keysetContext = query.KeysetPaginate(keysetBuilderAction, KeysetPaginationDirection.Forward);
        //    }

        //    // Execute pagination query
        //    //var dataQuery = keysetContext.Query
        //    //    .Take(request.First.Value);

        //    //foreach(var item in dataQuery) 
        //    //{
        //    //    data.Add(_pluginService.GetPluginDto(item));
        //    //}

        //    data = await keysetContext.Query
        //        .Take(request.First.Value)
        //        .Select(e => _pluginService.GetPluginDto(e))
        //        .ToListAsync();

        //    keysetContext.EnsureCorrectOrder(data);


        //    var result = new QueryBasePayload<List<PluginDto>>()
        //    {
        //        Count = count,
        //        Result = data,
        //        HasNext = await keysetContext.HasNextAsync(data),
        //        HasPrevious = await keysetContext.HasPreviousAsync(data)
        //    };

        //    //return _mapper.ProjectTo<PluginDto>(query, null).ToList();
        //    return result;

        //}


        public async Task<QueryCursorPaginatedPayload<PluginDto>> Handle(GetPluginRequest request, CancellationToken cancellationToken)
        {

            await using var dbContext = _factory.CreateDbContext();

            var query = dbContext.Plugins.AsQueryable();

            // Filtering data
            var filter = ExpressionFilterFactory.Create<Domain.Models.Plugin>();
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
                    After = request.After,
                    Before = request.Before,
                    Size = request.First
                }
                );

            return new QueryCursorPaginatedPayload<PluginDto>
            {
                Count = result.TotalCount,
                Result = result.Data,
                HasNext = result.HasNext,
                HasPrevious = result.HasPrevious
            };

        }
    }
}
