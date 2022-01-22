using FluentValidation.TestHelper;
using Inventory.API.Application.Configuration.Locations;
using Inventory.Domain.Models.Configuration;
using Inventory.UnitTests.SeedWork.Configuration;
using MediatR;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.UnitTests.API.Application.Configuration
{
    [TestFixture]
    public class LocationTest : BaseDbInventoryTests
    {

        private readonly IMediator _mediator;

        public LocationTest()
        {
            _mediator = this.GetMediator();
        }

        [TestCaseSource(typeof(LocationSeed.TestCases), nameof(LocationSeed.TestCases.GetLocationDtoWithValidValues))]
        public async Task Should_successfull_create_location_with_valid_values(CreateLocation.Command newEntity)
        {
            // act
            var actual = await _mediator.Send(newEntity);

            // Assert
            Assert.NotNull(actual);
        }

        [TestCaseSource(typeof(LocationSeed.TestCases), nameof(LocationSeed.TestCases.GetLocationDtoWithEmptyMandatoryValues))]
        public void Should_create_location_fails_when_code_is_empty_or_null(CreateLocation.Command newEntity)
        {

            // Arrange
            var repository = this.GetAsyncRepository<Location>();
            var validator = new CreateLocation.Validator(repository);

            // act
            var actual = validator.TestValidate(newEntity);

            // Assert
            actual.ShouldHaveValidationErrorFor(e => e.CityCode);
            actual.ShouldHaveValidationErrorFor(e => e.CountryCode);
            actual.ShouldHaveValidationErrorFor(e => e.Name);
        }

        [TestCaseSource(typeof(LocationSeed.TestCases), nameof(LocationSeed.TestCases.GetLocationDtoWithCityCodeAlreadExists))]
        public void Should_create_location_fails_when_code_already_exists(CreateLocation.Command newEntity)
        {

            // Arrange
            var repository = this.GetAsyncRepository<Location>();
            var validator = new CreateLocation.Validator(repository);

            // act
            var actual = validator.TestValidate(newEntity);

            // Assert
            actual.ShouldHaveValidationErrorFor(e => e.CityCode).WithErrorCode("LOC-04");
        }

        [TestCaseSource(typeof(LocationSeed.TestCases), nameof(LocationSeed.TestCases.GetLocationDtoWithBadValidToDate))]
        public void Should_create_location_fails_when_ValidTo_is_less_or_equal_than_ValidFrom(CreateLocation.Command newEntity)
        {

            // Arrange
            var repository = this.GetAsyncRepository<Location>();
            var validator = new CreateLocation.Validator(repository);

            // act
            var actual = validator.TestValidate(newEntity);

            // Assert
            actual.ShouldHaveValidationErrorFor(e => e.ValidTo).WithErrorCode("LOC-05");
        }



    }
}
