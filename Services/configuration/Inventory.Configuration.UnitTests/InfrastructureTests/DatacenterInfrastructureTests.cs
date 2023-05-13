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
    class DatacenterInfrastructureTests : DbUnitTests
    {
        [Test]
        public async Task Should_successfull_query_DatacenterEntity()
        {

            // Arrange
            var repo = UnitTestsContext.Current.GetAsyncRepository<Datacenter>();

            // Act
            var datacenters = await repo.ListAsync();

            // Assert
            Assert.IsNotNull(datacenters);

        }

    }
}
