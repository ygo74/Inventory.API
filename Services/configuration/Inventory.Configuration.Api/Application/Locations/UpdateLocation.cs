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
    /// Update Location request
    /// </summary>
    public class UpdateLocationRequest : UpdateConfigurationEntityRequest<LocationDto> 
    {
        public string Description { get; set; }
    }

    public class UpdateLocationValidator : ConfigurationEntityDtoValidator<UpdateLocationRequest>
    {
        public UpdateLocationValidator(ILocationService service) 
        {
            RuleFor(e => e.Id).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("{PropertyName} is mandatory")
                .MustAsync(service.LocationExists).WithMessage("Location with {PropertyName} {PropertyValue} doesn't exists in the database");
        }
    }

    /// <summary>
    /// CRUD Handler for locations
    /// </summary>
    public class UpdateLocationHanlder : IRequestHandler<UpdateLocationRequest, Payload<LocationDto>>
    {

        private readonly ILogger<UpdateLocationHanlder> _logger;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Location> _repository;

        public UpdateLocationHanlder(ILogger<UpdateLocationHanlder> logger, IMapper mapper, IAsyncRepository<Location> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = Guard.Against.Null(repository, nameof(repository));
        }

        /// <summary>
        /// Update location
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<LocationDto>> Handle(UpdateLocationRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start updating Location '{request.Id}'");

            // Find entity
            var location = await _repository.GetByIdAsync(request.Id,cancellationToken);

            if (request.Deprecated.HasValue) { location.SetDeprecatedValue(request.Deprecated.Value); }

            // Update location
            var changes = await _repository.UpdateAsync(location, cancellationToken);
            if (changes <= 0)
            {
                var error = $"Error updating Location '{request.Id}'";
                _logger.LogError(error);
                return Payload<LocationDto>.Error(new GenericApiError(error));
            }

            // return entity
            _logger.LogInformation($"Successfully updating Location '{request.Id}'");
            return Payload<LocationDto>.Success(_mapper.Map<LocationDto>(location));
        }

    }
}
