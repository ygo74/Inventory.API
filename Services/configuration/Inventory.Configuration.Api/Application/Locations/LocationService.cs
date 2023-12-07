using Ardalis.GuardClauses;
using Inventory.Configuration.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Inventory.Common.Domain.Filters;
using Inventory.Configuration.Domain.Filters;

namespace Inventory.Configuration.Api.Application.Locations
{
    public class LocationService : ILocationService
    {
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;
        private readonly ILogger<LocationService> _logger;


        public LocationService(IDbContextFactory<ConfigurationDbContext> factory, ILogger<LocationService> logger) 
        {
            _factory = Guard.Against.Null(factory, nameof(factory));
            _logger = Guard.Against.Null(logger, nameof(logger));

        }

        public async Task<bool> LocationExists(
                   int id,
                   CancellationToken cancellationToken)
        {

            await using ConfigurationDbContext dbContext =
                _factory.CreateDbContext();

            return (await dbContext.Locations.FindAsync(new object[] { id }, cancellationToken) != null);
        }

        /// <summary>
        /// Check if a location exists
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="cityCode"></param>
        /// <param name="regionCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> LocationExists(string countryCode,
                                               string cityCode,
                                               string regionCode,
                                               CancellationToken cancellationToken = default)
        {

            // check arguments are not null with GuardClauses
            Guard.Against.NullOrEmpty(countryCode, nameof(countryCode));
            Guard.Against.NullOrEmpty(cityCode, nameof(cityCode));
            Guard.Against.NullOrEmpty(regionCode, nameof(regionCode));

            // create Filter
            var filter = ExpressionFilterFactory.Create<Domain.Models.Location>()
                            .WithCityCode(cityCode)
                            .WithCountryCode(countryCode)
                            .WithRegionCode(regionCode);

            await using ConfigurationDbContext dbContext = _factory.CreateDbContext();

            return (await dbContext.Locations.AnyAsync(filter.Predicate, cancellationToken));
        }


    }
}
