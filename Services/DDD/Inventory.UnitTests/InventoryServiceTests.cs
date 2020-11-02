using Inventory.Domain;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Infrastructure.Databases;
using Inventory.Infrastructure.Databases.Repositories;
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


        public InventoryServiceTests() : base()
        {
            _groupRepository = new EfRepository<Group>(this.DbContext);
            _serverRepository = new EfRepository<Server>(this.DbContext);
            _osRepository = new EfRepository<Domain.Models.OperatingSystem>(this.DbContext);
            _envRepository = new EfRepository<Domain.Models.Environment>(this.DbContext);
            _serverGroupRepository = new EfRepository<ServerGroup>(this.DbContext);

        }


        [Test]
        public async Task GetServerByIdAsyncTest()
        {

            var svc = new InventoryService(_serverRepository, _groupRepository, _osRepository, _envRepository, _serverGroupRepository ,this.Logger);

            var server = await svc.GetServerByIdAsync(2);

            //Debug.WriteLine(server.Variables.RootElement);

            Assert.AreEqual(2, server.ServerId);
        }

        [Test]
        public async Task AddServerAsyncTest()
        {

            var svc = new InventoryService(_serverRepository, _groupRepository, _osRepository, _envRepository, _serverGroupRepository, this.Logger);

            string hostName = $"srv-{Guid.NewGuid()}";
            var server = await svc.AddServerAsync(hostName, OsFamilly.Windows, "windows 2016", "prd");

            var test = server.GetAnsibleVariables();

            //Debug.WriteLine(server.Variables.RootElement);

            Assert.IsTrue(server.ServerId > 0);
        }


    }

}