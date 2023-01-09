using Elastic.Apm.Api;
using Inventory.Common.Domain.Filters;
using Inventory.Configuration.Api.Application.Locations;
using Inventory.Configuration.Api.Application.Plugin;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using Inventory.Configuration.UnitTests.TestCases;
using MediatR;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Inventory.Configuration.Domain.Filters;

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
            Assert.IsTrue(result.Data.Id > 0);
        }




    }
}
