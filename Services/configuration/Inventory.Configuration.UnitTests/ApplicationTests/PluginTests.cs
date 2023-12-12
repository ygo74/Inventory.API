using Inventory.Configuration.Api.Application.Plugins;
using Inventory.Configuration.UnitTests.SeedWork;
using Inventory.Configuration.UnitTests.TestCases;
using MediatR;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.ApplicationTests
{
    [TestFixture]
    public class PluginTests : DbUnitTests
    {
        private readonly IMediator _mediator;

        public PluginTests()
        {
            _mediator = UnitTestsContext.Current.GetMediator();
        }

        [TestCaseSource(typeof(PluginTestCases), nameof(PluginTestCases.GetCreatePluginsWithCorrectMandatoryValues))]
        public async Task Should_successfull_create_application_with_valid_values(CreatePluginRequest newEntity)
        {
            // Arrange
            //var result = Assert.Throws<Inventory.Common.Application.Exceptions.ValidationException>(async () => 
            //{
            //    await _mediator.Send(newEntity);
            //});

            var result = await _mediator.Send(newEntity);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestCaseSource(typeof(PluginTestCases),nameof(PluginTestCases.GetPluginsByCodeAndName))]
        public async Task Should_successfull_get_plugin_by_code_and_name(GetPluginRequest request)
        {
            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(request.Code, result.Data[0].Code);
        }

        [Test]
        public async Task Should_successfull_get_plugin_by_id()
        {

            // Arrange
            var request = new GetPluginByIdRequest
            {
                Id = 1
            };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(request.Id, result.Data.Id);
        }

        [Test]
        public async Task Should_successfull_get_plugin_by_code()
        {

            // Arrange
            var request = new GetPluginByCodeRequest
            {
                Code = PluginSeed.AZURE_INVENTORY
            };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(request.Code, result.Data.Code);
        }

        [TestCase(1, "Azure Inventory", "AZURE_INVENTORY", "AZURE_INVENTORY", "Azure Inventory", 
                  "C:\\Program Files\\Inventory\\Plugins\\AzureInventory.dll", false, "2021-01-01", "2021-12-31")]
        public async Task Should_successfull_update_plugin(int id, string name, string code, string inventoryCode, string description,
                                                           string path, bool? deprecated = null, DateTime? validFrom = null, DateTime? validTo = null)
        {

            // Arrange
            var request = new UpdatePluginRequest
            {
                Id = id,
                //Name = "Azure Inventory",
                //Code = "AZURE_INVENTORY",
                Path = path,
                InventoryCode = inventoryCode,
                Deprecated = deprecated,
                ValidFrom = validFrom,
                ValidTo = validTo                
            };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(request.Id, result.Data.Id);
            //Assert.AreEqual(request.Name, result.Data.Name);
            //Assert.AreEqual(request.Code, result.Data.Code);
            //Assert.AreEqual(request.Path, result.Data.Path);
        }

    }
}
