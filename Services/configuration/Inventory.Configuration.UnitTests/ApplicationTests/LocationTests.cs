using Inventory.Common.Domain.Filters;
using Inventory.Configuration.Api.Application.Locations;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using Inventory.Configuration.UnitTests.TestCases;
using MediatR;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Configuration.Domain.Filters;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.UnitTests.SeedWork;

namespace Inventory.Configuration.UnitTests.ApplicationTests
{
    [TestFixture]
    public class LocationTests : DbUnitTests
    {
        private readonly IMediator _mediator;

        public LocationTests()
        {
            _mediator = UnitTestsContext.Current.GetMediator();
        }

        [TestCaseSource(typeof(LocationTestCases), nameof(LocationTestCases.GetCreateLocationWithCorrectValues))]
        public async Task Should_successfull_create_location_with_valid_values(CreateLocationRequest newEntity)
        {            
            // Act
            var result = await _mediator.Send(newEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.Id > 0);
        }

        [TestCaseSource(typeof(LocationTestCases), nameof(LocationTestCases.GetUpdateLocationWithCorrectValues))]
        public async Task Should_successfull_update_location_with_valid_values(string cityCode, string countryCode, string regionCode,
            string description, string inventoryCode, DateTime? validFrom, DateTime? validTo)
        {

            // Arrange
            var filter = ExpressionFilterFactory.Create<Location>();
            filter = filter.WithCityCode(cityCode);
            filter = filter.WithCountryCode(countryCode); 
            filter = filter.WithRegionCode(regionCode);

            var dbContext = UnitTestsContext.Current.GetService<ConfigurationDbContext>();
            var foundLocation = dbContext.Locations.First(filter.Predicate);

            var updateEntity = new UpdateLocationRequest()
            {
                Id = foundLocation.Id,
                Description = description,
                InventoryCode = inventoryCode,
                ValidFrom = validFrom,
                ValidTo = validTo
            };

            // Act
            var result = await _mediator.Send(updateEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Errors);
            Assert.IsTrue(result.Data.Id > 0);
        }


        [Test]
        public async Task Should_successfull_delete_location()
        {

            // Arrange
            var queryStore = UnitTestsContext.Current.GetService<IGenericQueryStore<Location>>();
            var existingLocation = await queryStore.FirstOrDefaultAsync(criteria: ExpressionFilterFactory.Create<Location>()
                                                                                                         .WithName(LocationSeed.Name_To_Be_Deleted));
            
            var request = new DeleteLocationRequest()
            {
                Id = existingLocation.Id
            };
            
            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Errors);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task Should_successfull_get_location_by_id()
        {

            var request = new GetLocationByIdRequest()
            {
                Id = 1
            };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Errors);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(request.Id, result.Data.Id);
        }

        [Test]
        public async Task Should_successfull_get_location_by_filter()
        {

            var request = new GetLocationRequest()
            {
                CityCode = LocationSeed.CityCode_London,
                CountryCode = LocationSeed.CountryCode_UK,
                RegionCode = LocationSeed.RegionCode_EMEA
            };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Errors);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count > 0);
            StringAssert.AreEqualIgnoringCase(LocationSeed.CityCode_London, result.Data[0].CityCode);
            StringAssert.AreEqualIgnoringCase(LocationSeed.CountryCode_UK, result.Data[0].CountryCode);
            StringAssert.AreEqualIgnoringCase(LocationSeed.RegionCode_EMEA, result.Data[0].RegionCode);
        }

        [Test]
        public async Task Should_successfull_get_location_by_name()
        {

            var request = new GetLocationByNameRequest()
            {
                Name = LocationSeed.Name_London
            };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Errors);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(LocationSeed.Name_London, result.Data.Name);
        }
        

    }
}
