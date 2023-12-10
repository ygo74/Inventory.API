using FluentValidation.TestHelper;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Datacenters;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.UnitTests.SeedWork;
using Inventory.Configuration.UnitTests.TestCases;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public DatacenterTests()
        {
            _mediator = UnitTestsContext.Current.GetMediator();
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
            var repository = UnitTestsContext.Current.GetService<IGenericQueryStore<Datacenter>>();
            var existingDatacenter = await repository.GetByIdAsync<DatacenterDto>(1);
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

    }
}
