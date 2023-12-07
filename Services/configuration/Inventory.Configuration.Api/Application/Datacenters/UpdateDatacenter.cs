using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Validators;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Api.Application.Datacenters.Validators;
using Inventory.Configuration.Api.Application.Locations;
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
        public string Description { get; set; }
        public string CountryCode { get; set; }
        public string CityCode { get; set; }
        public string RegionCode { get; set; }
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
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;

        public UpdateDatacenterHanlder(ILogger<UpdateDatacenterHanlder> logger, IMapper mapper, IDbContextFactory<ConfigurationDbContext> factory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _factory = Guard.Against.Null(factory, nameof(factory));
        }

        public async Task<Payload<DatacenterDto>> Handle(UpdateDatacenterRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start updating datacenter '{0}'", request.Id);

            // Find entity
            await using var dbContext = _factory.CreateDbContext();
            var datacenter = await dbContext.Datacenters.FindAsync(keyValues: new object[] { request.Id }, cancellationToken);

            // Update Property
            if (request.Deprecated.HasValue) { datacenter.SetDeprecatedValue(request.Deprecated.Value); }

            // Update location
            if (!string.IsNullOrWhiteSpace(request.RegionCode) && !string.IsNullOrWhiteSpace(request.CountryCode) &&
                    !string.IsNullOrWhiteSpace(request.CityCode))
            {


            }

            var changes = await dbContext.SaveChangesAsync(cancellationToken);
            if (changes > 0)
                _logger.LogInformation("Updated datacenter '{0}'", request.Id);

            return Payload<DatacenterDto>.Success(_mapper.Map<DatacenterDto>(datacenter));
        }
    }
}
