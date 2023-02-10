using FluentValidation.TestHelper;
using Inventory.Configuration.Api.Application.Datacenter;
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
            var actual = validator.TestValidate(newEntity);

            // Assert
            actual.ShouldHaveValidationErrorFor(e => e.Code);
            actual.ShouldHaveValidationErrorFor(e => e.Name);

        }

        [TestCaseSource(typeof(DatacenterTestCases), nameof(DatacenterTestCases.GetCreateDatacenterWithCorrectMandatoryValues))]
        public async Task Should_successfull_create_application_with_valid_values(CreateDatacenterRequest newEntity)
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

    }
}
