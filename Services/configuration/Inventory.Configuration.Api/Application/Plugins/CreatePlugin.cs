using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using HotChocolate;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Exceptions;
using Inventory.Common.Application.Validators;
using Inventory.Common.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Plugin
{
    public class CreatePlugin
    {
        [GraphQLName("CreatePluginInput")]
        public class Command : CreateConfigurationEntityRequest<PluginDto>
        {
            public string Name { get; set; }
            public string Code { get; set; }
            public string Version { get; set; }
            public string Path { get; set; }

        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {

                RuleFor(e => e.Name).Cascade(CascadeMode.Stop)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("{PropertyName} is mandatory");

                RuleFor(e => e.Code).Cascade(CascadeMode.Stop)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("{PropertyName} is mandatory");

                Include(new ConfigurationEntityDtoValidator<Command>());

            }
        }

        public class Handler : IRequestHandler<Command, Payload<PluginDto>>
        {

            private readonly IAsyncRepository<Domain.Models.Plugin> _repository;
            private readonly ILogger<Handler> _logger;
            private readonly IMapper _mapper;
            private readonly PluginService _pluginService;

            public Handler(IAsyncRepository<Domain.Models.Plugin> repository, ILogger<Handler> logger, IMapper mapper, PluginService pluginService)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _pluginService = Guard.Against.Null(pluginService, nameof(pluginService));
            }

            public async Task<Payload<PluginDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Start adding Plugin '{request.Name}' with code '{request.Code}'");

                bool success = false;
                try
                {
                    var newEntity = new Domain.Models.Plugin(request.Name, request.Code, request.Version, request.Deprecated, request.ValidFrom, request.ValidTo);
                    newEntity.SetPath(request.Path);

                    // Add entity
                    var result = await _repository.AddAsync(newEntity, cancellationToken);

                    // Map response
                    success = true;
                    return Payload<PluginDto>.Success(_pluginService.GetPluginDto(result));
                }
                finally
                {
                    if (success)
                        _logger.LogInformation($"Successfully adding Plugin '{request.Name}' with Code '{request.Code}' and version '{request.Version}'");
                    else
                        _logger.LogInformation($"Error when adding Plugin '{request.Name}' with Code '{request.Code}' and version '{request.Version}'");
                }

            }
        }

    }
}
