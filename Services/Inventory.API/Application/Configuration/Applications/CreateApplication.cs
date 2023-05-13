using Inventory.API.Application.Configuration.Base;
using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Models.Configuration;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading;

namespace Inventory.API.Application.Configuration.Applications
{
    public class CreateApplication
    {
        public class Command : CreateConfigurationEntity<ApplicationDto>
        {
            public string Name { get; set; }
            public string Code { get; set; }
        }

        internal class Validator : AbstractValidator<Command>
        {
            internal Validator()
            {

                RuleFor(e => e.Name).Cascade(CascadeMode.Stop)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("{PropertyName} is mandatory");

                RuleFor(e => e.Code).Cascade(CascadeMode.Stop)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("{PropertyName} is mandatory");

                When(e => e.ValidTo.HasValue && e.ValidFrom.HasValue, () =>
                {
                    RuleFor(e => e.ValidTo).Must((model, validTo) =>
                    {
                        return validTo.Value.CompareTo(model.ValidFrom.Value) > 0;
                    }).WithMessage("{PropertyName} with {PropertyValue} must be greather than ValidFrom date");
                });

            }
        }

        public class Handler : IRequestHandler<Command, ApplicationDto>
        {

            private readonly IAsyncRepository<Inventory.Domain.Models.Configuration.Application> _repository;
            private readonly ILogger<Handler> _logger;
            private readonly IMapper _mapper;

            public Handler(IAsyncRepository<Inventory.Domain.Models.Configuration.Application> repository, ILogger<Handler> logger, IMapper mapper)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<ApplicationDto> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Start adding application '{request.Name}' with code '{request.Code}'");

                try
                {
                    // Map request to Domain entity
                    var newEntity = _mapper.Map<Inventory.Domain.Models.Configuration.Application>(request);

                    // Add entity
                    var result = await _repository.AddAsync(newEntity, cancellationToken);

                    // Map response
                    return _mapper.Map<ApplicationDto>(result);

                }
                finally
                {
                    _logger.LogInformation($"End adding location '{request.Name}' with code '{request.Code}'");
                }

            }
        }


    }
}
