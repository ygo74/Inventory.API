using AutoMapper;
using FluentValidation;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Exceptions;
using Inventory.Configuration.Domain.Events;
using Inventory.Common.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Validators;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Inventory.Configuration.Infrastructure;
using Inventory.Common.Domain.Filters;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Domain.Filters;
using Inventory.Common.Application.Errors;

namespace Inventory.Configuration.Api.Application.Datacenters
{
    public class CreateDatacenterRequest : CreateConfigurationEntityRequest<DatacenterDto>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DatacenterTypeDto DatacenterType { get; set; }
        public string CountryCode { get; set; }
        public string CityCode { get; set; }
        public string RegionCode { get; set; }
    }

    public class CreateDatacenterValidator : CreateConfigurationEntityDtoValidator<CreateDatacenterRequest>
    {
        public CreateDatacenterValidator(IDbContextFactory<ConfigurationDbContext> factory)
        {

            // Mandatory attributes
            RuleFor(e => e.Name).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("{PropertyName} is mandatory")
                .NotEmpty().WithMessage("{PropertyName} is mandatory");

            RuleFor(e => e.Code).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is mandatory");

            RuleFor(e => e.DatacenterType).Cascade(CascadeMode.Stop)
                .NotNull()
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

            // Validate datacenter doesn't exists in the database with the same code
            RuleFor(e => e).Cascade(CascadeMode.Stop)
                .MustAsync(async (request, cancellation) =>
                {
                    using var dbContext = factory.CreateDbContext();

                    // create Filter
                    var filter = ExpressionFilterFactory.Create<Domain.Models.Datacenter>()
                                    .WithCode(request.Code);

                    var existingDatacenter = await dbContext.Datacenters.Where(filter.Predicate).FirstOrDefaultAsync(cancellation);
                    return (existingDatacenter == null);

                }).WithMessage("Datacenter with {PropertyName} {PropertyValue} already exists in the database");

            // validate location exists in the database with the three attributes LocationCountryCode, LocationCityCode, LocationRegionCode
            When(e => !string.IsNullOrEmpty(e.CountryCode) && !string.IsNullOrEmpty(e.CityCode) && !string.IsNullOrEmpty(e.RegionCode), () =>
            {
                RuleFor(e => e).Cascade(CascadeMode.Stop)
                    .MustAsync(async (request, cancellation) =>
                    {
                        using var dbContext = factory.CreateDbContext();

                        // create Filter
                        var filter = ExpressionFilterFactory.Create<Location>()
                                        .WithCityCode(request.CityCode)
                                        .WithCountryCode(request.CountryCode)
                                        .WithRegionCode(request.RegionCode);

                        var existingLocation = await dbContext.Locations.Where(filter.Predicate).FirstOrDefaultAsync(cancellation);
                        return (existingLocation != null);

                    }).WithMessage("Location with CountryCode {PropertyValue} and CityCode {PropertyValue} and RegionCode {PropertyValue} doesn't exists in the database");
            });
        }
    }

    public class CreateDatacenterHandler : IRequestHandler<CreateDatacenterRequest, Payload<DatacenterDto>>
    {

        private readonly IDbContextFactory<ConfigurationDbContext> _factory;
        private readonly ILogger<CreateDatacenterHandler> _logger;
        private readonly IMapper _mapper;

        public CreateDatacenterHandler(IDbContextFactory<ConfigurationDbContext> factory, ILogger<CreateDatacenterHandler> logger, IMapper mapper)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Add a new datacenter
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<DatacenterDto>> Handle(CreateDatacenterRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start adding datacenter '{0}' with code '{1}'", request.Name, request.Code);

            // Create datacenter
            var newEntity = new Domain.Models.Datacenter(request.Code, request.Name, Domain.Models.DatacenterType.Cloud, request.InventoryCode,
                                                        startDate: request.ValidFrom, endDate: request.ValidTo);

            // Find location and add it to the datacenter
            await using var dbContext = _factory.CreateDbContext();
            var filter = ExpressionFilterFactory.Create<Location>()
                            .WithCityCode(request.CityCode)
                            .WithCountryCode(request.CountryCode)
                            .WithRegionCode(request.RegionCode);

            var existingLocation = await dbContext.Locations.Where(filter.Predicate).FirstOrDefaultAsync(cancellationToken);
            if (existingLocation == null) 
            { 
                var errorMessage = $"Location with CountryCode {request.CountryCode} and CityCode {request.CityCode} and RegionCode {request.RegionCode} doesn't exists in the database";
                return Payload<DatacenterDto>.Error(new ValidationError(errorMessage));
            }

            newEntity.SetLocation(existingLocation);

            // Add entity
            var result =  await dbContext.Datacenters.AddAsync(newEntity, cancellationToken);
            var nbChanges = await dbContext.SaveChangesAsync(cancellationToken);
            if (nbChanges == 0)
            {
                var errorMessage = $"Error when adding datacenter '{request.Name}' with code '{request.Code}'";
                return Payload<DatacenterDto>.Error(new GenericApiError(errorMessage));
            }

            // return result
            _logger.LogInformation("Successfully added datacenter '{0}' with code '{1}'", request.Name, request.Code);
            return Payload<DatacenterDto>.Success(_mapper.Map<DatacenterDto>(newEntity));
        }
    }

}
