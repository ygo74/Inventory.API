using Inventory.Domain;
using Inventory.Domain.Models;
using Inventory.Domain.Models.ManagedEntities;
using Inventory.Domain.Specifications;
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
    public class EfRepositoryTests : BaseDbInventoryTests
    {

        //[Test]
        public async Task GetAllEnvironmentsTest()
        {

            var repo = new EfRepository<Inventory.Domain.Models.Configuration.Environment>(this.DbContext);
            var environments = await repo.ListAsync();

            Assert.IsTrue(environments.Any());
        }


        //[Test]
        public async Task GetServerByIdAsyncTest()
        {

            var repo = new EfRepository<Server>(this.DbContext);
            var serverRef = this.DbContext.Servers.FirstOrDefault();

            var serverCheck = await repo.GetByIdAsync(serverRef.ServerId);

            Assert.AreEqual(serverRef.ServerId, serverCheck.ServerId);
        }

        //[Test]
        public async Task GetAllServersTest()
        {

            var repo = new EfRepository<Server>(this.DbContext);
            var servers = await repo.ListAsync();

            Assert.IsTrue(servers.Any());
        }

        //[Test]
        public async Task GetAllApplicationsTest()
        {
            var repo = new EfRepository<Application>(this.DbContext);
            var applications = await repo.ListAsync();

            Assert.IsTrue(applications.Any());
        }

        //[Test]
        public async Task GetApplicationsByNameTest()
        {

            var appSpec = new ApplicationSpecification();
            appSpec.Name = "MyApp1";
            var repo = new EfRepository<Application>(this.DbContext);
            var applications = await repo.ListAsync(appSpec);

            Assert.IsTrue(applications.Any());
        }

        //[Test]
        public async Task GetApplicationsByNameAndCodeTest()
        {

            var appSpec = new ApplicationSpecification();
            appSpec.Name = "MyApp1";
            appSpec.Code = "ap1";
            var repo = new EfRepository<Application>(this.DbContext);
            var applications = await repo.ListAsync(appSpec);

            Assert.IsTrue(applications.Any());
        }

    }

}