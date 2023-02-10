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
    public class UpdatePluginRequest : UpdateConfigurationEntityRequest<PluginDto>
    {
        public string? Path { get; set; }
    }
#nullable disable


    public class UpdatePluginValidator : ConfigurationEntityDtoValidator<UpdatePluginRequest>
    {

        public UpdatePluginValidator(PluginService service)
        {

            RuleFor(e => e.Id).Cascade(CascadeMode.Stop)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("{PropertyName} is mandatory and must be greather than 0")
                .MustAsync(service.PluginExist).WithMessage("Plugin with id {PropertyValue} doesn't exist");
        }

    }

    public class UpdatePluginHanlder : IRequestHandler<UpdatePluginRequest, Payload<PluginDto>>
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

        public async Task<Payload<PluginDto>> Handle(UpdatePluginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Update plugin {0}", request.Id);
            bool success = false;
            try
            {

                await using var dbContext = _factory.CreateDbContext();

                var plugin = await dbContext.Plugins.FindAsync(new object[] { request.Id },cancellationToken);

                if (request.Deprecated.HasValue) { plugin.SetDeprecatedValue(request.Deprecated.Value); }
                if (!string.IsNullOrWhiteSpace(request.Path)) { plugin.SetPath(request.Path); }

                var changes = await dbContext.SaveChangesAsync(cancellationToken);

                success = true;
                return Payload<PluginDto>.Success(_pluginService.GetPluginDto(plugin));
            }
            finally
            {
                if (success)
                    _logger.LogInformation("Successfully Updating plugin {0}", request.Id);
                else
                    _logger.LogError("Error when updating plugin {0}", request.Id);

            }


        }
    }

}
