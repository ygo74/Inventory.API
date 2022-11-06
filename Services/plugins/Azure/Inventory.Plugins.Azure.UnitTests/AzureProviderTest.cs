using Inventory.Plugins.Azure.Models;
using Inventory.Plugins.Azure.Services;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Plugins.Azure.UnitTests
{
    [TestFixture]
    public class AzureProviderTest : AzureExecutionContext
    {

        [Test]
        public async Task Should_work()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.TestDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets(typeof(Inventory.Plugins.Azure.UnitTests.AzureProviderTest).Assembly)
                .AddEnvironmentVariables()
                .Build();

            var subscriptionId = Guid.Parse(configuration["Azure:SubscriptionId"]);
            var tenantId = Guid.Parse(configuration["Azure:TenantId"]);
            var clientId = Guid.Parse(configuration["Azure:ClientId"]);
            var password = configuration["Azure:ClientSecret"];

            var credential = new AzCredential("unittest", "account for unit tests");
            credential.SetAzurePassword(subscriptionId, tenantId, clientId, password);


            // Act
            var provider = new AzSubnetProvider(this.GetLogger<AzSubnetProvider>(), this.GetMapper());
            provider.SetCredential(credential);
            var results = await provider.ListAllAsync();
            
            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);

        }
    }
}
