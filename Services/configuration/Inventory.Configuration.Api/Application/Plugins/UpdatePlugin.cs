using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Errors;
using Inventory.Common.Application.Validators;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Api.Application.Plugins.Dtos;
using Inventory.Configuration.Api.Application.Plugins.Services;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using Inventory.Networks.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Plugins
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
        private readonly IAsyncRepository<Plugin> _repository;

        public UpdatePluginHanlder(IAsyncRepository<Plugin> repository,
            ILogger<UpdatePluginHanlder> logger, IMapper mapper, PluginService pluginService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _pluginService = Guard.Against.Null(pluginService, nameof(pluginService));
            _repository = Guard.Against.Null(repository, nameof(repository));
        }

        public async Task<Payload<PluginDto>> Handle(UpdatePluginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Update plugin {0}", request.Id);

            // Find plugin by Id
            var plugin = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (request.Deprecated.HasValue) { plugin.SetDeprecatedValue(request.Deprecated.Value); }
            if (!string.IsNullOrWhiteSpace(request.Path)) { plugin.SetPath(request.Path); }

            var changes = await _repository.SaveChangesAsync(cancellationToken);

            if (changes <= 0)
            {
                var errorMessage = $"Error when updating plugin {request.Id}";
                _logger.LogError(errorMessage);
                return Payload<PluginDto>.Error(new GenericApiError(errorMessage));

            }

            _logger.LogInformation("Successfully Updating plugin {0}", request.Id);
            return Payload<PluginDto>.Success(_pluginService.GetPluginDto(plugin));


        }
    }

}
