using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Validators;
using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Api.Application.Datacenters.Validators;
using Inventory.Configuration.Api.Application.Locations;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenters
{
    /// <summary>
    /// Update Datacenter request
    /// </summary>
    public class UpdateDatacenterRequest : UpdateConfigurationEntityRequest<DatacenterDto>, IDatacenterLocation
    {
        public string Name { get; set; }
        public DatacenterTypeDto? DatacenterType { get; set; }
        public string Description { get; set; } = null;
        public string CountryCode { get; set; } = null;
        public string CityCode { get; set; } = null;
        public string RegionCode { get; set; } = null;
    }

    public class UpdateDatacenterValidator : ConfigurationEntityDtoValidator<UpdateDatacenterRequest>
    {
        public UpdateDatacenterValidator(DatacenterService datacenterService, ILocationService locationService)
        {
            RuleFor(e => e.Id).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("{PropertyName} is mandatory")
                .MustAsync(datacenterService.DatacenterExists).WithMessage("Datacenter with {PropertyName} {PropertyValue} doesn't exists in the database");

            // Check if location exists in the database with the three attributes LocationCountryCode, LocationCityCode, LocationRegionCode
            Include(new DatacenterExistValidator(locationService));
        }
    }

    /// <summary>
    /// Update Handler for Datacenter
    /// </summary>
    public class UpdateDatacenterHanlder : IRequestHandler<UpdateDatacenterRequest, Payload<DatacenterDto>>
    {

        private readonly ILogger<UpdateDatacenterHanlder> _logger;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Datacenter> _dcRepository;
        private readonly IAsyncRepository<Location> _locationRepository;

        public UpdateDatacenterHanlder(ILogger<UpdateDatacenterHanlder> logger, IMapper mapper, 
                                       IAsyncRepository<Datacenter> dcRepository, IAsyncRepository<Location> locationRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dcRepository = Guard.Against.Null(dcRepository, nameof(dcRepository));
            _locationRepository = Guard.Against.Null(locationRepository, nameof(locationRepository));
            
        }

        public async Task<Payload<DatacenterDto>> Handle(UpdateDatacenterRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start updating datacenter '{0}'", request.Id);

            // Find entity
            var datacenter = await _dcRepository.GetByIdAsync(request.Id, cancellationToken,
                                                                e => e.Location, e => e.Plugins);

            // Update Property
            if (request.Deprecated.HasValue) { datacenter.SetDeprecatedValue(request.Deprecated.Value); }
            if (request.Description != null) { datacenter.SetDescription(request.Description); }
            if (!string.IsNullOrWhiteSpace(request.InventoryCode)) { datacenter.SetInventoryCode(request.InventoryCode); }
            if (request.DatacenterType is not null) { 
                // convert enum DatacenterTypeDto to DatacenterType by using standard enum parsing
                var datacenterType = Enum.Parse<DatacenterType>(request.DatacenterType.ToString());
                datacenter.SetDatacenterType(datacenterType);
            }



            // Update location
            if (!string.IsNullOrWhiteSpace(request.RegionCode) && !string.IsNullOrWhiteSpace(request.CountryCode) &&
                    !string.IsNullOrWhiteSpace(request.CityCode))
            {
                // find the location thanks to regionCode, countrycode and citycode by using ExpressionFilter and locationRepository, then update the datacenter location
                var locationFilter = ExpressionFilterFactory.Create<Location>()
                                                                .WithRegionCode(request.RegionCode)
                                                                .WithCountryCode(request.CountryCode)
                                                                .WithCityCode(request.CityCode);    

                var location = await _locationRepository.FirstOrDefaultAsync(locationFilter, cancellationToken: cancellationToken);
                datacenter.SetLocation(location);

            }

            var changes = await _dcRepository.SaveChangesAsync(cancellationToken);
            if (changes > 0)
                _logger.LogInformation("Updated datacenter '{0}'", request.Id);

            return Payload<DatacenterDto>.Success(_mapper.Map<DatacenterDto>(datacenter));
        }
    }
}
