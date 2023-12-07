using Ardalis.GuardClauses;
using FluentValidation;
using Inventory.Common.Domain.Filters;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Inventory.Configuration.Api.Application.Locations;

namespace Inventory.Configuration.Api.Application.Datacenters.Validators
{
    public class DatacenterExistValidator : AbstractValidator<IDatacenterLocation>
    {
        // Add this constructor with IdbcontextFactory<ConfigurationDbContext> as parameter
        public DatacenterExistValidator(ILocationService locationService)
        {
            // Check that locationService is not null
            Guard.Against.Null(locationService, nameof(locationService));

            // validate location exists in the database with the three attributes LocationCountryCode, LocationCityCode, LocationRegionCode
            When(e => !string.IsNullOrEmpty(e.CountryCode) && !string.IsNullOrEmpty(e.CityCode) && !string.IsNullOrEmpty(e.RegionCode), () =>
            {
                RuleFor(e => e).Cascade(CascadeMode.Stop)
                    .MustAsync(async (request, cancellation) =>
                    {
                        return await locationService.LocationExists(countryCode: request.CountryCode,
                                                              cityCode: request.CityCode,
                                                              regionCode: request.RegionCode,
                                                              cancellationToken: cancellation);

                    }).WithMessage("Location with CountryCode {PropertyValue} and CityCode {PropertyValue} and RegionCode {PropertyValue} doesn't exists in the database");
            });


        }
    }
}
