using Inventory.Configuration.Domain.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.InfrastructureTests
{
    [TestFixture]
    class PluginInfrastructureTests : DbUnitTests
    {
        [Test]
        public async Task Should_successfull_query_pluginEntity()
        {

            // Arrange
            var repo = UnitTestsContext.Current.GetAsyncRepository<Plugin>();

            // Act
            var plugins = await repo.ListAllAsync();

            // Assert
            Assert.IsNotNull(plugins);
            Assert.IsNotEmpty(plugins);

        }
    }
}
