using AutoMapper;
using FluentValidation;
using Inventory.Api.Base.Dto;
using Inventory.Api.Base.Validators;
using Inventory.Devices.Api.Applications.OperatingSystem.Dto;
using Inventory.Devices.Domain.Models;
using Inventory.Domain.Base.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Devices.Api.Applications.OperatingSystem
{
    public class CreateOperatingSystem
    {

        public class Command2 : CreateConfigurationEntityDto<OperatingSystemDto>
        {
            public OperatingSystemFamilyDto OperatingSystemFamily { get; set; }
            public string Model { get; set; }
            public string Version { get; set; }
        }

        public class Validator : AbstractValidator<Command2>
        {
            public Validator()
            {

                RuleFor(e => e.OperatingSystemFamily).Cascade(CascadeMode.Stop)
                    .NotNull()
                    .NotEmpty()
                    .Must(e => e > 0)
                    .WithMessage("{PropertyName} is mandatory");

                RuleFor(e => e.Model).Cascade(CascadeMode.Stop)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("{PropertyName} is mandatory");

                RuleFor(e => e.Version).Cascade(CascadeMode.Stop)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("{PropertyName} is mandatory");

                Include(new ConfigurationEntityDtoValidator<Command2>());

            }
        }

        public class Handler : IRequestHandler<Command2, OperatingSystemDto>
        {

            private readonly IAsyncRepository<Inventory.Devices.Domain.Models.OperatingSystem> _repository;
            private readonly ILogger<Handler> _logger;
            private readonly IMapper _mapper;

            public Handler(IAsyncRepository<Inventory.Devices.Domain.Models.OperatingSystem> repository, ILogger<Handler> logger, IMapper mapper)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<OperatingSystemDto> Handle(Command2 request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Start adding Operating system '{request.OperatingSystemFamily}' with Model '{request.Model}' and version '{request.Version}'");

                bool success = false;
                try
                {
                    // Map request to Domain entity
                    var osFamily = OperatingSystemFamily.FromValue< OperatingSystemFamily>((int)request.OperatingSystemFamily);
                    var newEntity = new Inventory.Devices.Domain.Models.OperatingSystem(osFamily, request.Model, request.Version, 
                                                                                        request.Deprecated, request.ValidFrom, request.ValidTo);

                    // Add entity
                    var result = await _repository.AddAsync(newEntity, cancellationToken);

                    // Map response
                    var resultDto = _mapper.Map<OperatingSystemDto>(result);
                    success = true;
                    return resultDto;

                }
                finally
                {
                    if (success)
                        _logger.LogInformation($"Successfully adding Operating system '{request.OperatingSystemFamily}' with Model '{request.Model}' and version '{request.Version}'");
                    else
                        _logger.LogInformation($"Error when adding Operating system '{request.OperatingSystemFamily}' with Model '{request.Model}' and version '{request.Version}'");
                }

            }
        }


    }
}
