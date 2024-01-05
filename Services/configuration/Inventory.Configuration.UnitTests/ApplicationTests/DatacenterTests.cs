using FluentValidation.TestHelper;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Credentials;
using Inventory.Configuration.Api.Application.Datacenters;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Api.Application.Datacenters.Validators;
using Inventory.Configuration.Api.Application.Locations.Services;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using Inventory.Configuration.UnitTests.SeedWork;
using Inventory.Configuration.UnitTests.TestCases;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.ApplicationTests
{
    [TestFixture]
    class DatacenterTests : DbUnitTests
    {

        private readonly IMediator _mediator;
        private DatacenterExistValidator _validator;
        private Mock<ILocationService> _locationServiceMock;


        private class DatacenterLocationDto : IDatacenterLocation
        {
            public string CountryCode { get; set; }
            public string CityCode { get; set; }
            public string RegionCode { get; set; }
        }

        public DatacenterTests()
        {
            _mediator = UnitTestsContext.Current.GetMediator();
        }

        [SetUp]
        public void Setup()
        {
            _locationServiceMock = new Mock<ILocationService>();
            _validator = new DatacenterExistValidator(_locationServiceMock.Object);
        }

        [Test]
        public async Task Validate_WhenLocationExists_ShouldNotHaveValidationError()
        {
            // Arrange
            var datacenterLocation = new DatacenterLocationDto
            {
                CountryCode = "US",
                CityCode = "NYC",
                RegionCode = "NY"
            };

            _locationServiceMock.Setup(x => x.LocationExists(datacenterLocation.CountryCode, datacenterLocation.CityCode, datacenterLocation.RegionCode, default))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(datacenterLocation);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.That(result.Errors, Is.Empty);

        }

        [Test]
        public async Task Validate_WhenLocationDoesNotExist_ShouldHaveValidationError()
        {
            // Arrange
            var datacenterLocation = new DatacenterLocationDto
            {
                CountryCode = "US",
                CityCode = "NYC",
                RegionCode = "NY"
            };

            _locationServiceMock.Setup(x => x.LocationExists(datacenterLocation.CountryCode, datacenterLocation.CityCode, datacenterLocation.RegionCode, default))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(datacenterLocation);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors, Is.Not.Empty);
        }


        [TestCaseSource(typeof(DatacenterTestCases), nameof(DatacenterTestCases.GetCreateDatacenterWithEmptyMandatoryValues))]
        public async Task Should_throws_validation_exeption_when_empty_values(CreateDatacenterRequest newEntity)
        {
            //// Arrange
            ////var result = Assert.Throws<Inventory.Common.Application.Exceptions.ValidationException>(async () => 
            ////{
            ////    await _mediator.Send(newEntity);
            ////});

            //var result = await _mediator.Send(newEntity);

            //// Assert
            //Assert.IsNotNull(result);

            // Arrange
            var validator = UnitTestsContext.Current.GetService<CreateDatacenterValidator>();

            // Act
            var actual = await validator.TestValidateAsync(newEntity);

            // Assert
            actual.ShouldHaveValidationErrorFor(e => e.Code);
            actual.ShouldHaveValidationErrorFor(e => e.Name);

        }

        [TestCase("Datacenter-1", "DC1", "dc.dc1", LocationSeed.RegionCode_EMEA, LocationSeed.CountryCode_France, LocationSeed.CityCode_Paris,
                  DatacenterTypeDto.OnPremise)]
        [TestCase("Datacenter-2", "DC2", "dc.dc2", LocationSeed.RegionCode_EMEA, LocationSeed.CountryCode_France, LocationSeed.CityCode_Paris,
                  DatacenterTypeDto.OnPremise, false, "Test full attributes", null , null )]
        public async Task Should_successfull_create_application_with_valid_values(string name, string code, string inventoryCode,
                                                                                  string regionCode, string countryCode, string cityCode,
                                                                                  DatacenterTypeDto datacenterType, bool? deprecated = null,
                                                                                  string description=null, DateTime? validFrom=null,
                                                                                  DateTime? validTo = null)
        {
            // Arrange
            var newEntity = new CreateDatacenterRequest()
            {
                Name = name,
                Code = code,
                DatacenterType = datacenterType,
                InventoryCode = inventoryCode,
                CountryCode = countryCode,
                CityCode = cityCode,
                RegionCode = regionCode,
                Description = description,
                ValidFrom = validFrom,  
                ValidTo = validTo,
                Deprecated = deprecated                
            };

            // Act
            var result = await _mediator.Send(newEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsEmpty(result.Errors);
        }

        [TestCase("new description","new description")]
        [TestCase(null, "new description")]
        [TestCase("", null)]
        public async Task Should_successfull_update_datacenter_description(string updateDescriptionValue, string expectedDescriptionValue)
        {
            // Arrange
            var queryStore = UnitTestsContext.Current.GetService<IGenericQueryStore<Datacenter>>();

            var existingDatacenter = await queryStore.GetByIdAsync<DatacenterDto>(1);
            var updateRequest = new UpdateDatacenterRequest()
            {
                Id = 1,
                Description = updateDescriptionValue
            };
            
            // Act
            var result = await _mediator.Send(updateRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDescriptionValue, result.Data.Description);
            // manage case null to inform that this field should not be updated
            if (updateDescriptionValue != null) Assert.AreNotEqual(existingDatacenter.Description, result.Data.Description);
        }


        [Test]
        public async Task Should_successfull_get_datacenter_by_id()
        {

            // Arrange
            var dbContext = UnitTestsContext.Current.GetService<ConfigurationDbContext>();
            var foundDatacenter = dbContext.Datacenters.First();
            var request = new GetDatacenterByIdRequest { Id = foundDatacenter.Id };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(foundDatacenter.Id, result.Data.Id);
        }


        [Test]
        public async Task Should_successfull_get_datacenter_by_code()
        {

            // Arrange
            var dbContext = UnitTestsContext.Current.GetService<ConfigurationDbContext>();
            var foundDatacenter = dbContext.Datacenters.First();
            var request = new GetDatacenterByCodeRequest { Code = foundDatacenter.Code };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(foundDatacenter.Code, result.Data.Code);
        }

        [Test]
        public async Task Should_successfull_get_datacenter_by_name()
        {

            // Arrange
            var dbContext = UnitTestsContext.Current.GetService<ConfigurationDbContext>();
            var foundDatacenter = dbContext.Datacenters.First();
            var request = new GetDatacenterByNameRequest { Name = foundDatacenter.Name };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(foundDatacenter.Name, result.Data.Name);
        }

        [Test]
        public async Task Should_successfull_get_datacenter_plugin_endpoints()
        {
            // Arrange
            var dbContext = UnitTestsContext.Current.GetService<ConfigurationDbContext>();
            var datacenterIds = dbContext.Datacenters.Select<Datacenter, int>(e => e.Id).ToList();
            var request = new GetPluginsByDatacenterIdRequest
            {
                DatacenterIds = datacenterIds
            };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public async Task Should_successfull_set_datacenter_plugin_endpoints()
        {
            // Arrange
            var dbContext = UnitTestsContext.Current.GetService<ConfigurationDbContext>();
            var datacenter = dbContext.Datacenters.First(e => e.Code == DataCenterSeed.DATACENTER_PARIS_CODE);
            var plugin = dbContext.Plugins.First(e => e.Code == PluginSeed.EFFICIENTIP_INVENTORY);
            var credential = dbContext.Credentials.First(e => e.Name == CredentialSeed.TO_BE_DELETED);

            var request = new SetDatacenterPluginEndpointRequest
            {
                DatacenterCode = datacenter.Code,
                CredentialName = credential.Name,
                PluginCode = plugin.Code
            };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Errors);
            Assert.IsNotEmpty(result.Data);
        }




    }
}
