using Ardalis.GuardClauses;
using Inventory.Configuration.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;

namespace Inventory.Configuration.Api.Application.Locations
{
    public class LocationService
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


    }
}
