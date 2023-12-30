using Ardalis.GuardClauses;
using Inventory.Configuration.Infrastructure;
using Inventory.Configuration.Domain.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Inventory.Common.Domain.Filters;

namespace Inventory.Configuration.Api.Application.Credentials.Services
{
    public class CredentialService : ICredentialService
    {
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;
        private readonly ILogger<CredentialService> _logger;

        public CredentialService(IDbContextFactory<ConfigurationDbContext> factory, ILogger<CredentialService> logger)
        {
            _factory = Guard.Against.Null(factory, nameof(factory));
            _logger = Guard.Against.Null(logger, nameof(logger));
        }

        /// <summary>
        /// Validate if a credential exists with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> CredentialExists(int id,
                                                 CancellationToken cancellationToken)
        {

            await using ConfigurationDbContext dbContext =
                _factory.CreateDbContext();

            return await dbContext.Credentials.FindAsync(new object[] { id }, cancellationToken) != null;
        }

        /// <summary>
        /// Validate if a credential exists with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> CredentialExists(string name,
                                                 CancellationToken cancellationToken = default)
        {

            Guard.Against.NullOrEmpty(name, nameof(name));

            var filter = ExpressionFilterFactory.Create<Domain.Models.Credential>()
                            .WithName(name);

            await using ConfigurationDbContext dbContext =
                _factory.CreateDbContext();

            return await dbContext.Credentials.AnyAsync(filter.Predicate, cancellationToken);
        }

    }
}
