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
    /// Delete location request
    /// </summary>
    public class DeleteLocationRequest : DeleteConfigurationEntityRequest<LocationDto> { }


    public class DeleteLocationValidator : AbstractValidator<DeleteLocationRequest>
    {

        public DeleteLocationValidator(LocationService service)
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
    /// Delete Handler for location
    /// </summary>
    public class DeleteLocationHanlder : IRequestHandler<DeleteLocationRequest, Payload<LocationDto>>
    {

        private readonly ILogger<DeleteLocationHanlder> _logger;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;

        public DeleteLocationHanlder(ILogger<DeleteLocationHanlder> logger, IMapper mapper, IDbContextFactory<ConfigurationDbContext> factory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _factory = Guard.Against.Null(factory, nameof(factory));
        }

        /// <summary>
        /// Remove location
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<LocationDto>> Handle(DeleteLocationRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start deleting Location '{request.Id}'");

            bool success = false;
            try
            {
                // Find entity
                await using var dbContext = _factory.CreateDbContext();
                var location = await dbContext.Locations.FindAsync(request.Id);

                if (null == location)
                    return Payload<LocationDto>.Error(new NotFoundError($"Don't find Location with Id {request.Id}"));

                // delete location
                dbContext.Locations.Remove(location);
                var changes = await dbContext.SaveChangesAsync(cancellationToken);

                if (changes <= 0)
                    return Payload<LocationDto>.Error();

                success = true;
                return Payload<LocationDto>.Success(default(LocationDto));
            }
            finally
            {
                if (success)
                    _logger.LogInformation($"Successfully deleting Location '{request.Id}'");
                else
                    _logger.LogInformation($"Error when deleting Location '{request.Id}'");
            }
        }
    }
}
