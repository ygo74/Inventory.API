using Ardalis.GuardClauses;
using Inventory.Common.Domain.Filters;
using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenters.Services
{
    public class DatacenterService : IDatacenterService
    {
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;
        private readonly ILogger<DatacenterService> _logger;

        // add constructor with IDbContextFactory<ConfigurationDbContext> and ILogger<DatacenterService>
        public DatacenterService(IDbContextFactory<ConfigurationDbContext> factory, ILogger<DatacenterService> logger)
        {
            // add GuardClauses to store factory and logger
            _factory = Guard.Against.Null(factory, nameof(factory));
            _logger = Guard.Against.Null(logger, nameof(logger));

        }

        /// <summary>
        /// Check if a datacenter exists
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DatacenterExists(int id,
                                                 CancellationToken cancellationToken)
        {

            // check if datacenter exists in the database
            await using ConfigurationDbContext dbContext = _factory.CreateDbContext();
            return await dbContext.Datacenters.FindAsync(new object[] { id }, cancellationToken) != null;
        }

        /// <summary>
        /// Check if a datacenter exists
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DatacenterExists(string code,
                                                 CancellationToken cancellationToken = default)
        {

            // check arguments are not null with GuardClauses
            Guard.Against.NullOrEmpty(code, nameof(code));

            // create Filter
            var filter = ExpressionFilterFactory.Create<Domain.Models.Datacenter>()
                            .WithCode(code);

            // Check if datacenter exists in the database
            await using ConfigurationDbContext dbContext = _factory.CreateDbContext();
            return await dbContext.Datacenters.AnyAsync(filter.Predicate, cancellationToken);
        }



    }
}
