using Inventory.Domain.Models.Credentials;
using Inventory.Infrastructure.Azure.Services;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.UnitTests.Infrastructure
{
    [TestFixture]
    public class AzureProviderTest : BaseDbInventoryTests
    {

        [Test]
        public async Task Should_work()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.TestDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets(typeof(Inventory.UnitTests.Infrastructure.AzureProviderTest).Assembly)
                .AddEnvironmentVariables()
                .Build();

            var subscriptionId = Guid.Parse(configuration["Azure:SubscriptionId"]);
            var tenantId = Guid.Parse(configuration["Azure:TenantId"]);
            var clientId = Guid.Parse(configuration["Azure:ClientId"]);
            var password = configuration["Azure:ClientSecret"];

            var credential = new AzureCredential("unittest", "account for unit tests");
            credential.SetAzurePassword(subscriptionId, tenantId, clientId, password);


            // Act
            var provider = new NetworkService(this.GetLogger<NetworkService>(), this.GetMapper(), credential);
            var results = await provider.ListAllAsync();
            
            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);

        }
    }
}
