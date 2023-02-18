using FluentValidation.TestHelper;
using HotChocolate.Execution;
using Inventory.Common.Application.Exceptions;
using Inventory.Devices.Api.Applications.OperatingSystem;
using Inventory.Devices.Api.Configuration;
using Inventory.Devices.UnitTests.TestCases;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Devices.UnitTests.Tests.OperatingSystemTests
{
    [TestFixture]
    class CreateOperatingSystemTest : BaseDbInventoryTests
    {

        private readonly IMediator _mediator;

        public CreateOperatingSystemTest()
        {
            _mediator = this.GetMediator();
        }

        public override void AddCustomService(ServiceCollection services, IWebHostEnvironment Environment)
        {
            services.AddGraphqlServices(Environment);
            base.AddCustomService(services, Environment);
        }

        [TestCaseSource(typeof(OperatingSystemTestCases), nameof(OperatingSystemTestCases.GetOperatingSystemWithMissingMandatoryValues))]
        public void Should_detect_create_operating_system_with_missing_mandatory_values(CreateOperatingSystem.Command2 newEntity)
        {
            // Arrange
            var validator = this.GetService<CreateOperatingSystem.Validator>();

            // Act
            var testResult = validator.TestValidate(newEntity);

            // Assert
            if (newEntity.OperatingSystemFamily == 0)
            {
                testResult.ShouldHaveValidationErrorFor(e => e.OperatingSystemFamily);
            }
            else if (string.IsNullOrWhiteSpace(newEntity.Model))
            {
                testResult.ShouldHaveValidationErrorFor(e => e.Model);
            }
            else if (string.IsNullOrWhiteSpace(newEntity.Version))
            {
                testResult.ShouldHaveValidationErrorFor(e => e.Version);
            }
            else
            {
                Assert.Fail("Should detect validation error");
            }

        }

        [TestCaseSource(typeof(OperatingSystemTestCases), nameof(OperatingSystemTestCases.GetOperatingSystemWithBadValues))]
        public void Should_detect_create_operating_system_with_bad_values(CreateOperatingSystem.Command2 newEntity)
        {
            // Arrange
            var validator = this.GetService<CreateOperatingSystem.Validator>();

            // Act
            var testResult = validator.TestValidate(newEntity);

            // Assert
            testResult.ShouldHaveValidationErrorFor(e => e.ValidTo);

        }

        [TestCaseSource(typeof(OperatingSystemTestCases), nameof(OperatingSystemTestCases.GetOperatingSystemWithMissingMandatoryValues))]
        public async Task Should_fail_create_operating_system_with_missing_mandatory_values(CreateOperatingSystem.Command2 newEntity)
        {
            // Act
            var result = await _mediator.Send(newEntity);

            // Assert
            Assert.IsTrue(result.Errors.Any());
        }

        [TestCaseSource(typeof(OperatingSystemTestCases), nameof(OperatingSystemTestCases.GetOperatingSystemWithBadValues))]
        public async Task  Should_fail_create_operating_system_with_bad_values(CreateOperatingSystem.Command2 newEntity)
        {
            // Act
            var result = await _mediator.Send(newEntity);

            // Assert
            Assert.IsTrue(result.Errors.Any());

        }


        [TestCaseSource(typeof(OperatingSystemTestCases), nameof(OperatingSystemTestCases.GetOperatingSystemWithValidValues))]
        public async Task Should_successfull_create_operating_system_with_valid_values(CreateOperatingSystem.Command2 newEntity)
        {
            // act
            var actual = await _mediator.Send(newEntity);

            // Assert
            Assert.NotNull(actual);
        }


        [Test]
        public async Task Should_successfull_execute_mutation()
        {

            // Arrange
            var executor = await this.GetExecutor();

            // act
            IExecutionResult result = await executor.ExecuteAsync(@"
                mutation createOs {
                  createOperatingSystem(command: {
                    operatingSystemFamily: AIX
                    model: ""AIX""
                    version: ""11""
                    deprecated: false
                  }) 
                  {
                    data {
                        operatingSystemFamily
                        model
                        version
                    }
                  }
                }");

            // assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Errors);

        }



    }
}
