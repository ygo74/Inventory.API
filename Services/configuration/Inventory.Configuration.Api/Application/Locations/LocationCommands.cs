using Ardalis.GuardClauses;
using AutoMapper;
using GreenDonut;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
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

    /// <summary>
    /// Update Location request
    /// </summary>
    public class UpdateLocationRequest : UpdateConfigurationEntityRequest<LocationDto> 
    {
        public string Description { get; set; }
    }

    /// <summary>
    /// Delete location request
    /// </summary>
    public class DeleteLocationRequest : DeleteConfigurationEntityRequest<LocationDto> { }


    /// <summary>
    /// CRUD Handler for locations
    /// </summary>
    public class CreateLocationHanlder : IRequestHandler<CreateLocationRequest, Payload<LocationDto>>,
                                         IRequestHandler<UpdateLocationRequest, Payload<LocationDto>>,
                                         IRequestHandler<DeleteLocationRequest, Payload<LocationDto>>
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
                    return Payload<LocationDto>.Error();

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
