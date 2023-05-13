using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using GreenDonut;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Exceptions;
using Inventory.Common.Application.Validators;
using Inventory.Configuration.Api.Application.Plugin;
using Inventory.Configuration.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Locations
{

    /// <summary>
    /// Create location request
    /// </summary>
    public class CreateLocationRequest : CreateConfigurationEntityRequest<LocationDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string CountryCode { get; set; }
        public string CityCode { get; set; }
        public string RegionCode { get; set; }

    }

    public class CreateLocationValidator : CreateConfigurationEntityDtoValidator<CreateLocationRequest>
    {
        public CreateLocationValidator() 
        {
            RuleFor(e => e.Name).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is mandatory");

            RuleFor(e => e.CountryCode).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is mandatory");

            RuleFor(e => e.CityCode).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is mandatory");

            RuleFor(e => e.RegionCode).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is mandatory");
        }
    }

    /// <summary>
    /// CRUD Handler for locations
    /// </summary>
    public class CreateLocationHanlder : IRequestHandler<CreateLocationRequest, Payload<LocationDto>>
    {

        private readonly ILogger<CreateLocationHanlder> _logger;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;

        public CreateLocationHanlder(ILogger<CreateLocationHanlder> logger, IMapper mapper, IDbContextFactory<ConfigurationDbContext> factory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _factory = Guard.Against.Null(factory, nameof(factory));
        }

        /// <summary>
        /// Create location
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<LocationDto>> Handle(CreateLocationRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start adding Location '{request.Name}' with City code '{request.CityCode}', Country code '{request.CountryCode}'");

            bool success = false;
            try
            {
                var newEntity = new Domain.Models.Location(request.Name, request.CountryCode, request.CityCode, request.RegionCode, request.InventoryCode,
                                                           "", request.Deprecated, request.ValidFrom, request.ValidTo);

                // Add entity
                await using var dbContext = _factory.CreateDbContext();
                var result = await dbContext.Locations.AddAsync(newEntity, cancellationToken);
                var nbChanges = await dbContext.SaveChangesAsync(cancellationToken);

                // Map response
                var resultDto = _mapper.Map<LocationDto>(newEntity); 

                success = true;
                return Payload<LocationDto>.Success(resultDto);
            }
            finally
            {
                if (success)
                    _logger.LogInformation($"Successfully adding Plugin '{request.Name}' with City Code '{request.CityCode}', country code '{request.CountryCode}'");
                else
                    _logger.LogInformation($"Error when adding Plugin '{request.Name}' with City Code '{request.CityCode}', country code '{request.CountryCode}'");
            }
        }

    }
}
