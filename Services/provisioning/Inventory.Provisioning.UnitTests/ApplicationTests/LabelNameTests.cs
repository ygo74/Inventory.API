using Inventory.Provisioning.Api.Applications.LabelNames;
using Inventory.Provisioning.Infrastructure;
using Inventory.Provisioning.UnitTests.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Provisioning.UnitTests.ApplicationTests
{
    [TestFixture]
    class LabelNameTests : DbUnitTests
    {

        private readonly IMediator _mediator;

        public LabelNameTests()
        {
            _mediator = UnitTestsContext.Current.GetMediator();
        }


        [TestCase(LabelNameSeed.LABEL_OS_FAMILY)]
        [TestCase("zzzz")]
        public async Task Should_successfuly_get_label_by_name(string name)
        {
            // Arange
            var queryByName = new GetLabelNameByName() { Name = name };

            // Act
            var result = await _mediator.Send(queryByName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.Count == 0);
            Assert.AreEqual(name, result.Data.Name);

        }

        [TestCase(LabelNameSeed.LABEL_OS_FAMILY)]
        public async Task Should_successfuly_get_label_by_id(string name)
        {
            // Arange
            var labelEntity = UnitTestsContext.Current.GetService<ProvisioningDbContext>().LabelNames.Single(e => e.Name == name);
            var queryById = new GetLabelNameById() { Id = labelEntity.Id };

            // Act
            var result = await _mediator.Send(queryById);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.Count == 0);
            Assert.AreEqual(name, result.Data.Name);

        }

    }
}
