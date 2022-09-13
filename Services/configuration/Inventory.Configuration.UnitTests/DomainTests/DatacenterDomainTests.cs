using Inventory.Configuration.Domain.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.DomainTests
{
    [TestFixture]
    class DatacenterDomainTests
    {
        [Test]
        public void Should_successfull_Create_DatacenterEntity()
        {

            // Arrange

            // Act
            var datacenter = new Datacenter("TEST", "test", DatacenterType.Cloud);

            // Assert
            Assert.IsNotNull(datacenter);

        }

    }
}
