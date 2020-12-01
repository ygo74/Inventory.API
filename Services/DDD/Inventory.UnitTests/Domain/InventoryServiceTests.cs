using Inventory.Domain;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Specifications;
using Inventory.Infrastructure.Databases;
using Inventory.Infrastructure.Databases.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Inventory.UnitTests
{
    public class InventoryServiceTests : BaseInventoryTests<InventoryService>
    {

        private readonly IAsyncRepository<Server> _serverRepository;
        private readonly IAsyncRepository<Group> _groupRepository;
        private readonly IAsyncRepository<Domain.Models.OperatingSystem> _osRepository;
        private readonly IAsyncRepository<Domain.Models.Environment> _envRepository;
        private readonly IAsyncRepository<ServerGroup> _serverGroupRepository;
        private readonly Mock<IMediator> _mediator;
        private readonly InventoryService _inventoryService;


        public InventoryServiceTests() : base()
        {
            _groupRepository = new EfRepository<Group>(this.DbContext);
            _serverRepository = new EfRepository<Server>(this.DbContext);
            _osRepository = new EfRepository<Domain.Models.OperatingSystem>(this.DbContext);
            _envRepository = new EfRepository<Domain.Models.Environment>(this.DbContext);
            _serverGroupRepository = new EfRepository<ServerGroup>(this.DbContext);
            _mediator = new Mock<IMediator>();

            _inventoryService = new InventoryService(_serverRepository, _groupRepository, _osRepository, _envRepository, _serverGroupRepository, this.Logger, _mediator.Object);

        }

        #region Operating Systems
        [Test]
        public async Task GetorAddOperatingSystemByNameAsyncTest()
        {
            var os = await _inventoryService.GetorAddOperatingSystemByNameAsync(OsFamilly.Linux, "rhel_8");
            Assert.IsNotNull(os);
        }
        #endregion

        [Test]
        public async Task GetServerByIdAsyncTest()
        {

            var serverRef = await _serverRepository.FirstOrDefaultAsync(new ServerSpecification());

            var serverCheck = await _inventoryService.GetServerByIdAsync(serverRef.ServerId);
            Assert.AreEqual(serverRef.ServerId, serverCheck.ServerId);

        }

        [Test]
        public async Task AddServerAsyncTest()
        {
            string hostName = $"srv-{Guid.NewGuid()}";
            var server = await _inventoryService.AddServerAsync(hostName, OsFamilly.Windows, "windows 2016", "prd", System.Net.IPAddress.Parse("10.0.0.1"));

            Assert.IsTrue(server.ServerId > 0);
        }


    }

}