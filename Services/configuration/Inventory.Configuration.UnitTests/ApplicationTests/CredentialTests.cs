using Inventory.Configuration.Api.Application.Credentials;
using Inventory.Configuration.Api.Application.Credentials.Services;
using Inventory.Configuration.Infrastructure;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.ApplicationTests
{
    [TestFixture]
    class CredentialTests : DbUnitTests
    {
        private readonly IMediator _mediator;
        private CreateCredentialRequestValidator _validator;


        public CredentialTests()
        {
            _mediator = UnitTestsContext.Current.GetMediator();
        }

        [SetUp]
        public void Setup()
        {
            // Initialize the validator with the necessary dependencies
            var service = new Mock<ICredentialService>().Object;
            _validator = new CreateCredentialRequestValidator(service);
        }

        [Test]
        public async Task Name_ShouldBeMandatory_when_create_credential()
        {
            // Arrange
            var request = new CreateCredentialRequest();

            // Act
            var result = await _validator.ValidateAsync(request);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Name is mandatory"));
        }

        [Test]
        public async Task Name_ShouldNotAlreadyExistInDatabase_when_create_credential()
        {
            // Arrange
            var request = new CreateCredentialRequest { Name = "TestCredential" };
            var service = new Mock<ICredentialService>();
            service.Setup(s => s.CredentialExists(request.Name, CancellationToken.None)).ReturnsAsync(true);
            _validator = new CreateCredentialRequestValidator(service.Object);

            // Act
            var result = await _validator.ValidateAsync(request);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Credential's name with value TestCredential already exists in the database"));
        }

        [Test]
        public async Task Should_successfull_get_credential_by_id()
        {
            // Arrange
            var dbContext = UnitTestsContext.Current.GetService<ConfigurationDbContext>();
            var foundCredential = dbContext.Credentials.First();
            var request = new GetCredentialByIdRequest { Id = foundCredential.Id };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(foundCredential.Id, result.Data.Id);
        }

        [Test]
        public async Task Should_successfull_get_credential_by_name()
        {
            // Arrange
            var dbContext = UnitTestsContext.Current.GetService<ConfigurationDbContext>();
            var foundCredential = dbContext.Credentials.First();
            var request = new GetCredentialByNameRequest { Name = foundCredential.Name };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(foundCredential.Id, result.Data.Id);
        }


        [TestCase("Test credential","Test credential description")]
        public async Task Should_successfull_create_credential_with_valid_values(string name, string description)
        {
            // Arrange
            var newEntity = new CreateCredentialRequest()
            {
                Name = name,
                Description = description
            };

            // Act
            var result = await _mediator.Send(newEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.Id > 0);
        }
    }
}
