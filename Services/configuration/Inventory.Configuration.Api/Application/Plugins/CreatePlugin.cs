using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using HotChocolate;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Errors;
using Inventory.Common.Application.Exceptions;
using Inventory.Common.Application.Validators;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Plugin
{
    /// <summary>
    /// Create plugin request
    /// </summary>
    public class CreatePluginRequest : CreateConfigurationEntityRequest<PluginDto>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Version { get; set; }
        public string Path { get; set; }

    }


    /// <summary>
    /// Create plugin request validator
    /// </summary>
    public class CreatePluginValidator : CreateConfigurationEntityDtoValidator<CreatePluginRequest>
    {
        public CreatePluginValidator()
        {

            RuleFor(e => e.Name).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is mandatory");

            RuleFor(e => e.Code).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is mandatory");

            RuleFor(e => e.Version).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is mandatory");

            RuleFor(e => e.Path).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is mandatory");

        }
    }

    /// <summary>
    /// Create plugin request Handler
    /// </summary>
    public class CreatePluginHanlder : IRequestHandler<CreatePluginRequest, Payload<PluginDto>>
    {

        private readonly IAsyncRepository<Domain.Models.Plugin> _repository;
        private readonly ILogger<CreatePluginHanlder> _logger;
        private readonly IMapper _mapper;
        private readonly PluginService _pluginService;

        public CreatePluginHanlder(IAsyncRepository<Domain.Models.Plugin> repository, ILogger<CreatePluginHanlder> logger, IMapper mapper, PluginService pluginService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _pluginService = Guard.Against.Null(pluginService, nameof(pluginService));
        }

        public async Task<Payload<PluginDto>> Handle(CreatePluginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start adding Plugin '{request.Name}' with code '{request.Code}'");

            var newEntity = new Domain.Models.Plugin(request.Name, request.Code, request.Version, request.InventoryCode, 
                                                     request.Deprecated, request.ValidFrom, request.ValidTo);
            newEntity.SetPath(request.Path);

            // Add entity
            var result = await _repository.AddAsync(newEntity, cancellationToken);
            if (result.nbchanges == 0)
            {
                var errorMessage = $"Error when adding Plugin '{request.Name}' with code '{request.Code}'";
                return Payload<PluginDto>.Error(new GenericApiError(errorMessage));
            }

            // return result
            _logger.LogInformation("Successfully added plugin '{0}' with code '{1}'", request.Name, request.Code);
            return Payload<PluginDto>.Success(_pluginService.GetPluginDto(newEntity));

        }
    }

}
