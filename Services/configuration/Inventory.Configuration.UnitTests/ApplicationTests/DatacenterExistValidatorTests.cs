using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Api.Application.Datacenters.Validators;
using Inventory.Configuration.Api.Application.Locations.Services;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.UnitTests.Datacenters.Validators
{
    [TestFixture]
    public class DatacenterExistValidatorTests
    {
        private DatacenterExistValidator _validator;
        private Mock<ILocationService> _locationServiceMock;

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

        private class DatacenterLocationDto : IDatacenterLocation
        {
            public string CountryCode { get; set; }
            public string CityCode { get; set; }
            public string RegionCode { get; set; }
        }
    }
}
