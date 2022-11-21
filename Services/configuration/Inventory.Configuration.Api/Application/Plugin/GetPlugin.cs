using Ardalis.GuardClauses;
using AutoMapper;
using Inventory.Configuration.Infrastructure;
using Inventory.Domain.Base.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Plugin
{
    public class GetPlugin : IRequest<PluginDto>
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Version { get; set; }
    }

    public class GetPluginHandler : IRequestHandler<GetPlugin, PluginDto>
    {
        private readonly IAsyncRepository<Domain.Models.Plugin> _repository;
        private readonly ILogger<GetPluginHandler> _logger;
        private readonly IMapper _mapper;
        private readonly PluginService _pluginService;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;

        public GetPluginHandler(IAsyncRepository<Domain.Models.Plugin> repository, ILogger<GetPluginHandler> logger,
            IMapper mapper, PluginService pluginService, IDbContextFactory<ConfigurationDbContext> factory )
        {
            _repository = Guard.Against.Null(repository,nameof(repository));
            _logger = Guard.Against.Null(logger, nameof(logger));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _pluginService = Guard.Against.Null(pluginService, nameof(pluginService));
            _factory = Guard.Against.Null(factory, nameof(factory));
        }

        public async Task<PluginDto> Handle(GetPlugin request, CancellationToken cancellationToken)
        {

            await using var dbContext = _factory.CreateDbContext();

            var query = dbContext.Plugins;

            Func<Domain.Models.Plugin, bool> x = new Func<Domain.Models.Plugin, bool>(e => e.Id == request.Id);
            System.Linq.Expressions.Expression<Func<Domain.Models.Plugin, bool>> z = e => e.Id == request.Id;

            if (request.Id.HasValue) { query.Where(e => e.Id == request.Id); }
            if (request.Id.HasValue) { query.Where(e => e.Id == request.Id); }

            throw new NotImplementedException();


        }
    }
}
