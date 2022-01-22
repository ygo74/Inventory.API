using NUnit.Framework;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using OperatingSystem = Inventory.Domain.Models.Configuration.OperatingSystem;
using Environment = Inventory.Domain.Models.Configuration.Environment;
using Inventory.Domain.Filters;
using Inventory.Domain.Enums;
using Inventory.Domain.Models.ManagedEntities;
using Inventory.UnitTests.SeedWork;
using Inventory.Domain.Specifications;
using Inventory.UnitTests.SeedWork.ManagedEntities;

namespace Inventory.UnitTests.Domain
{
    [TestFixture]
    public class ServerTest
    {


        [Test]
        public void Filter_by_Hostname()
        {

            // Arrange

            var servers = ServerSeed.Get();

            var filter = new ServerFilter()
            {
                Hostname = "srv2"
            };

            // Act
            var result = filter.ToSpecification().Evaluate(servers).SingleOrDefault();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(filter.Hostname, result.HostName);

        }

        [Test]
        public void Find_by_spec_osFamily_and_valid()
        {

            // Arrange
            var servers = ServerSeed.Get();
            var spec = new ServerByOSReadOnlySpec(OsFamily.Windows).And(new ServerByActiveStatusReadOnlySpec());

            // Act
            var result = spec.Evaluate(servers).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);

            var checkSpec = new ServerByOSReadOnlySpec(OsFamily.Windows);
            var resultCheck = checkSpec.Evaluate(servers).ToList();
            Assert.IsNotNull(resultCheck);
            Assert.IsTrue(resultCheck.Count > 0);

        }


    }
}
