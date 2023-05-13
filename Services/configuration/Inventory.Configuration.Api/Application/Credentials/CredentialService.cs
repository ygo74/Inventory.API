using Ardalis.GuardClauses;
using Inventory.Configuration.Api.Application.Locations;
using Inventory.Configuration.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;

namespace Inventory.Configuration.Api.Application.Credentials
{
    public class CredentialService
    {
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;
        private readonly ILogger<CredentialService> _logger;

        public CredentialService(IDbContextFactory<ConfigurationDbContext> factory, ILogger<CredentialService> logger) 
        {
            _factory = Guard.Against.Null(factory, nameof(factory));
            _logger = Guard.Against.Null(logger, nameof(logger));
        }

        public async Task<bool> CredentialExists(
                   int id,
                   CancellationToken cancellationToken)
        {

            await using ConfigurationDbContext dbContext =
                _factory.CreateDbContext();

            return (await dbContext.Credentials.FindAsync(new object[] { id }, cancellationToken) != null);
        }

    }
}
