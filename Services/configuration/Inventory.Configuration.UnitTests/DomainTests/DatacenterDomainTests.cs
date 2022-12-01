using Inventory.Configuration.Domain.Filters;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Domain.Specifications.DatacenterSpecifications;
using Inventory.Configuration.UnitTests.SeedWork;
using Inventory.Common.Domain.Filters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.DomainTests
{
    [TestFixture]
    class DatacenterDomainTests : DbUnitTests
    {
        [Test]
        public void Should_successfull_Create_DatacenterEntity()
        {

            // Arrange

            // Act
            var datacenter = new Datacenter("TEST", "test", DatacenterType.Cloud,"");

            // Assert
            Assert.IsNotNull(datacenter);

        }

        [Test]
        public void Should_successfull_Filter_DatacenterEntity()
        {

            // Arrange
            var datacenters = DataCenterSeed.Get();
            var criteria = ExpressionFilterFactory.Create<Datacenter>();
            criteria = criteria.WithCode("EMEA-FR-PARIS").Valid();
            var searchDatacenter = new DatacenterSearchSpecification(criteria);

            // Act
            var foundDatacenter = searchDatacenter.Evaluate(datacenters).Single();

            // Assert
            Assert.IsNotNull(foundDatacenter);
            Assert.AreEqual("EMEA-FR-PARIS", foundDatacenter.Code);

        }



    }

}
