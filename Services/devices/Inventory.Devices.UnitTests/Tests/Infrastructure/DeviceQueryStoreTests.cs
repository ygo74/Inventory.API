using AutoMapper;
using Inventory.Devices.Domain.Interfaces;
using Inventory.Devices.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperatingSystem = Inventory.Devices.Domain.Models.OperatingSystem;

namespace Inventory.Devices.UnitTests.Tests.Infrastructure
{

    class DeviceSummary
    {
        public string Hostname { get; set; }
        public string OperatingSystemVersion { get; set; }
    }


    [TestFixture]
    class DeviceQueryStoreTests : BaseDbInventoryTests
    {

        public override void AddCustomService(ServiceCollection services, IWebHostEnvironment Environment)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<Device, DeviceSummary>()
                    .ForMember(dest => dest.OperatingSystemVersion, opt => opt.MapFrom(src => ((Server)src).OperatingSystem.Version));
            });
            base.AddCustomService(services, Environment);
        }

        [Test]
        public async Task Should_get_all_devices()
        {
            // Arrange
            var queryStore = this.GetService<IDeviceQueryStore>();

            // Act
            var devices = await queryStore.ListAllAsync(e => ((Server)e).OperatingSystem);

            // Assert
            Assert.IsNotNull(devices);
        }

        [Test]
        public async Task Should_get_all_devices_with_projections()
        {
            // Arrange
            var _logger = this.GetLogger<DeviceQueryStoreTests>();
            _logger.LogDebug("TEST OUTPUT");
            var queryStore = this.GetService<IDeviceQueryStore>();

            // Act
            var devices = await queryStore.ListAllAsync<DeviceSummary>(e => ((Server)e).OperatingSystem);

            // Assert
            Assert.IsNotNull(devices);
        }


        //[Test]
        //public void Should_get_all_devices_with_os()
        //{
        //    // Arrange
        //    var queryStore = this.GetService<IDeviceQueryStore>();

        //    // Act
        //    var devices = queryStore.GetAllDevicesAsync<DeviceWithOperatingSystem>();

        //    // Assert
        //    Assert.IsNotNull(devices);
        //}

        [Test]
        public async Task Should_get_device_by_id()
        {
            // Arrange
            var queryStore = this.GetService<IDeviceQueryStore>();

            // Act
            var device = await queryStore.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(device);
        }

        [Test]
        public async Task Should_get_device_by_id_with_projections()
        {
            // Arrange
            var queryStore = this.GetService<IDeviceQueryStore>();

            // Act
            var device = await queryStore.GetByIdAsync<DeviceSummary>(1);

            // Assert
            Assert.IsNotNull(device);
        }

    }
}
