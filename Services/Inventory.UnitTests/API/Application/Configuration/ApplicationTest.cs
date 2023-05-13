using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.API.Application.Configuration.Applications;
using Inventory.UnitTests.SeedWork.Configuration;
using MediatR;
using NUnit.Framework;


namespace Inventory.UnitTests.API.Application.Configuration
{

    [TestFixture]
    internal class ApplicationTest : BaseDbInventoryTests
    {
        private readonly IMediator _mediator;

        public ApplicationTest()
        {
            _mediator = this.GetMediator();
        }

        [TestCaseSource(typeof(ApplicationSeed.TestCases), nameof(ApplicationSeed.TestCases.GetCreateApplicationWithValidValues))]
        public async Task Should_successfull_create_application_with_valid_values(CreateApplication.Command newEntity)
        {
            // act
            var actual = await _mediator.Send(newEntity);

            // Assert
            Assert.NotNull(actual);
        }



    }
}
