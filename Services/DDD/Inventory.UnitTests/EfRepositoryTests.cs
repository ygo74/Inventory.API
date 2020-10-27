using Inventory.Domain;
using Inventory.Domain.Models;
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
    public class EfRepositoryTests : BaseInventoryTests<EfRepository<Server>>
    {


        [Test]
        public async Task GetAllEnvironmentsTest()
        {

            var repo = new EfRepository<Domain.Models.Environment>(this.DbContext);
            var environments = await repo.ListAllAsync();

            Assert.IsTrue(environments.Any());
        }


        [Test]
        public async Task GetServerByIdAsyncTest()
        {

            var repo = new EfRepository<Server>(this.DbContext);
            var server = await repo.GetByIdAsync(1);

            Assert.AreEqual(1, server.ServerId);
        }

        [Test]
        public async Task GetAllServersTest()
        {

            var repo = new EfRepository<Server>(this.DbContext);
            var servers = await repo.ListAllAsync();

            Assert.IsTrue(servers.Any());
        }

        [Test]
        public async Task GetAllGroups()
        {
            var repo = new EfRepository<Group>(this.DbContext);
            var groups = await repo.ListAllAsync();

            Assert.IsTrue(groups.Any());
        }

    }

}