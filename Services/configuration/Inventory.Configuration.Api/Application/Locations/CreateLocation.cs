using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using GreenDonut;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Errors;
using Inventory.Common.Application.Validators;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Locations.Dtos;
using Inventory.Configuration.Api.Application.Locations.Services;
using Inventory.Configuration.Domain.Models;
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
        public CreateLocationValidator(ILocationService locationService) 
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

            RuleFor(e => e).Cascade(CascadeMode.Stop)
                .MustAsync(async (request, cancellation) =>
                {
                    return !await locationService.LocationExists(countryCode: request.CountryCode,
                                                          cityCode: request.CityCode,
                                                          regionCode: request.RegionCode,
                                                          cancellationToken: cancellation);

                }).WithMessage("Location with CountryCode {PropertyValue} and CityCode {PropertyValue} and RegionCode {PropertyValue} already exists in the database");


        }
    }

    /// <summary>
    /// CRUD Handler for locations
    /// </summary>
    public class CreateLocationHanlder : IRequestHandler<CreateLocationRequest, Payload<LocationDto>>
    {

        private readonly ILogger<CreateLocationHanlder> _logger;
        private readonly IAsyncRepository<Location> _repository;

        public CreateLocationHanlder(ILogger<CreateLocationHanlder> logger, IAsyncRepository<Location> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = Guard.Against.Null(repository, nameof(repository));
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

            var newEntity = new Location(request.Name, request.CountryCode, request.CityCode, request.RegionCode, request.InventoryCode,
                                         request.Description, request.Deprecated, request.ValidFrom, request.ValidTo);

            // Add entity
            var result = await _repository.AddAsync(newEntity, cancellationToken);
            if (result == 0)
            {
                var errorMessage = $"Error when adding Location '{request.Name}' with City code '{request.CityCode}', Country code '{request.CountryCode}'";
                _logger.LogInformation(errorMessage);
                return Payload<LocationDto>.Error(new GenericApiError(errorMessage));
            }

            // return result
            _logger.LogInformation("Successfully added Location '{0}' with City code '{1}', Country code '{2}'", request.Name, request.CityCode, request.CountryCode);
            return Payload<LocationDto>.Success(newEntity.ToLocationDto());

        }

    }
}
