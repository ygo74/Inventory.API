using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Validators;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MR.AspNetCore.Pagination;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Plugin
{
#nullable enable
    public class UpdatePluginRequest : UpdateConfigurationEntityDto<UpdatePluginPayLoad>
    {
        public string? Path { get; set; }
    }
#nullable disable

    public class UpdatePluginPayLoad : BasePayload<UpdatePluginPayLoad, IApiError>
    {
        public PluginDto Plugin { get; set; }

    }

    public class UpdatePluginValidator : AbstractValidator<UpdatePluginRequest>
    {
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;

        public UpdatePluginValidator(IDbContextFactory<ConfigurationDbContext> factory)
        {
            _factory = Guard.Against.Null(factory, nameof(factory));

            RuleFor(e => e.Id).Cascade(CascadeMode.Stop)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("{PropertyName} is mandatory and must be greather than 0")
                .MustAsync(PluginExist).WithMessage("Plugin with id {PropertyValue} doesn't exist");

            Include(new ConfigurationEntityDtoValidator<UpdatePluginRequest>());

        }

        public async Task<bool> PluginExist(UpdatePluginRequest request, int id, CancellationToken cancellationToken)
        {
            await using var dbContext = _factory.CreateDbContext();

            return await dbContext.Plugins.AnyAsync(e => e.Id == request.Id);
        }

    }

    public class UpdatePluginHanlder : IRequestHandler<UpdatePluginRequest, UpdatePluginPayLoad>
    {
        private readonly ILogger<UpdatePluginHanlder> _logger;
        private readonly IMapper _mapper;
        private readonly PluginService _pluginService;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;

        public UpdatePluginHanlder(IDbContextFactory<ConfigurationDbContext> factory, 
            ILogger<UpdatePluginHanlder> logger, IMapper mapper, PluginService pluginService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _pluginService = Guard.Against.Null(pluginService, nameof(pluginService));
            _factory = Guard.Against.Null(factory, nameof(factory));
        }

        public async Task<UpdatePluginPayLoad> Handle(UpdatePluginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Update plugin {0}", request.Id);

            await using var dbContext = _factory.CreateDbContext();

            var plugin = await dbContext.Plugins.FindAsync(request.Id);

            if (request.Deprecated.HasValue) { plugin.SetDeprecatedValue(request.Deprecated.Value); }
            if (!string.IsNullOrWhiteSpace(request.Path)) { plugin.SetPath(request.Path); }

            var changes = await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("End Update plugin {0}", request.Id);

            return new UpdatePluginPayLoad
            {
                Plugin = _pluginService.GetPluginDto(plugin)
            };


        }
    }

}
