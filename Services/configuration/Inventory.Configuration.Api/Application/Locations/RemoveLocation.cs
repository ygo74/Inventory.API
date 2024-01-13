using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Errors;
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
    /// Delete location request
    /// </summary>
    public class DeleteLocationRequest : DeleteConfigurationEntityRequest<LocationDto> { }


    public class DeleteLocationValidator : AbstractValidator<DeleteLocationRequest>
    {

        public DeleteLocationValidator(ILocationService service)
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
        private readonly IAsyncRepository<Location> _repository;

        public DeleteLocationHanlder(ILogger<DeleteLocationHanlder> logger, IMapper mapper, IAsyncRepository<Location> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = Guard.Against.Null(repository, nameof(repository));
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

            // Find entity
            var location = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (null == location)
                return Payload<LocationDto>.Error(new NotFoundError($"Don't find Location with Id {request.Id}"));

            // delete location
            var changes = await _repository.DeleteAsync(location, cancellationToken);
            if (changes <= 0)
            {
                var errorMessage = "Error when deleting Location '{request.Id}'";
                _logger.LogInformation(errorMessage);
                return Payload<LocationDto>.Error(new GenericApiError(errorMessage));
            }

            _logger.LogInformation($"Successfully deleted Location '{request.Id}'");
            return Payload<LocationDto>.Success(default(LocationDto));
        }
    }
}
