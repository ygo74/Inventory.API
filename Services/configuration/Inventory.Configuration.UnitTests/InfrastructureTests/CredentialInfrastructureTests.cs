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
    class CredentialInfrastructureTests : DbUnitTests
    {
        [Test]
        public async Task Should_successfull_query_CredentialEntity()
        {

            // Arrange
            var repo = UnitTestsContext.Current.GetAsyncRepository<Credential>();

            // Act
            var credentials = await repo.ListAsync();

            // Assert
            Assert.IsNotNull(credentials);
            Assert.IsTrue(credentials.Count > 0);

        }

    }
}
