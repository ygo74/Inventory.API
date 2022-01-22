using AutoMapper;
using FluentValidation;
using Inventory.Domain.Models.Configuration;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.API.Application.Configuration.Locations
{
    public class CreateLocation
    {

        public class Command : CreateConfigurationEntity<LocationDto>
        {
            public string CountryCode { get; set; }
            public string CityCode { get; set; }
            public string Name { get; set; }

        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAsyncRepository<Location> repository)
            {
                _ = repository ?? throw new ArgumentNullException(nameof(repository));

                // Mandatory values
                RuleFor(e => e.CityCode).NotNull().NotEmpty().WithMessage("{PropertyName} is mandatory").WithErrorCode("LOC-01");
                RuleFor(e => e.CountryCode).NotNull().NotEmpty().WithMessage("{PropertyName} is mandatory").WithErrorCode("LOC-02");
                RuleFor(e => e.Name).NotNull().NotEmpty().WithMessage("{PropertyName} is mandatory").WithErrorCode("LOC-03");

                // Not exist in Database
                When(e => !string.IsNullOrWhiteSpace(e.CityCode), () =>
                {
                    RuleFor(e => e.CityCode).MustAsync(async (code, cancellationToken) =>  
                    {
                        var spec = new LocationSpecification(code);
                        var existingLocation = await repository.FirstOrDefaultAsync(spec);
                        return (existingLocation == null);

                    }).WithMessage("{PropertyName} with {PropertyValue} already exists in Inventory").WithErrorCode("LOC-04");
                });

                // Valid dates
                When(e => e.ValidTo.HasValue && e.ValidFrom.HasValue, () =>
                {
                    RuleFor(e => e.ValidTo).Must((model, validTo) =>
                    {
                        return validTo.Value.CompareTo(model.ValidFrom.Value) > 0;
                    }).WithMessage("{PropertyName} with {PropertyValue} must be greather than ValidFrom date").WithErrorCode("LOC-05");
                });
                
            }
        }

        public class Handler : IRequestHandler<Command, LocationDto>
        {

            private readonly IAsyncRepository<Location> _repository;
            private readonly ILogger<Handler> _logger;
            private readonly IMapper _mapper;

            public Handler(IAsyncRepository<Location> repository, ILogger<Handler> logger, IMapper mapper)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<LocationDto> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Start Create location '{request.Name}' with CityCode '{request.CityCode}' in CountryCode '{request.CountryCode}'");
                try
                {
                    // Map request to Domain entity
                    var newEntity = _mapper.Map<Location>(request);

                    // Add entity
                    var result = await _repository.AddAsync(newEntity, cancellationToken);

                    // Map response
                    return _mapper.Map<LocationDto>(result);

                }
                finally
                {
                    _logger.LogInformation($"End Create location '{request.Name}' with CityCode '{request.CityCode}' in CountryCode '{request.CountryCode}'");
                }

            }
        }



    }
}
