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
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;

        public UpdateLocationHanlder(ILogger<UpdateLocationHanlder> logger, IMapper mapper, IDbContextFactory<ConfigurationDbContext> factory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _factory = Guard.Against.Null(factory, nameof(factory));
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

            bool success = false;
            try
            {
                // Find entity
                await using var dbContext = _factory.CreateDbContext();
                var location = await dbContext.Locations.FindAsync(request.Id);

                if (request.Deprecated.HasValue) { location.SetDeprecatedValue(request.Deprecated.Value); }

                // Update location
                var changes = await dbContext.SaveChangesAsync(cancellationToken);

                success = true;
                return Payload<LocationDto>.Success(_mapper.Map<LocationDto>(location));
            }
            finally
            {
                if (success)
                    _logger.LogInformation($"Successfully updating Location '{request.Id}'");
                else
                    _logger.LogInformation($"Error when updating Location '{request.Id}'");
            }
        }

    }
}
