using Inventory.Domain;
using Inventory.Domain.Extensions;
using Inventory.Domain.Models;
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
    public class EfRepositoryTests : BaseDbInventoryTests<EfRepository<Server>>
    {

        [Test]
        public async Task GetAllEnvironmentsTest()
        {

            var repo = new EfRepository<Inventory.Domain.Models.Environment>(this.DbContext);
            var environments = await repo.ListAllAsync();

            Assert.IsTrue(environments.Any());
        }


        [Test]
        public async Task GetServerByIdAsyncTest()
        {

            var repo = new EfRepository<Server>(this.DbContext);
            var serverRef = this.DbContext.Servers.FirstOrDefault();

            var serverCheck = await repo.GetByIdAsync(serverRef.ServerId);

            Assert.AreEqual(serverRef.ServerId, serverCheck.ServerId);
        }

        [Test]
        public async Task GetAllServersTest()
        {

            var repo = new EfRepository<Server>(this.DbContext);
            var servers = await repo.ListAllAsync();

            Assert.IsTrue(servers.Any());
        }

        [Test]
        public async Task GetAllGroupsTest()
        {
            var repo = new EfRepository<Group>(this.DbContext);
            var groups = await repo.ListAllAsync();

            Assert.IsTrue(groups.Any());
        }

        [Test]
        public void GetAllLinkedGroupsTest()
        {
            var repo = new GroupRepository(this.DbContext);
            var groups = repo.GetAllLinkedGroups("windows");

            Assert.IsTrue(groups.Any());
        }

        [Test]
        public async Task GetAllApplicationsTest()
        {
            var repo = new EfRepository<Application>(this.DbContext);
            var applications = await repo.ListAllAsync();

            Assert.IsTrue(applications.Any());
        }

        [Test]
        public async Task GetApplicationsByNameTest()
        {

            var appSpec = new ApplicationSpecification();
            appSpec.Name = "MyApp1";
            var repo = new EfRepository<Application>(this.DbContext);
            var applications = await repo.ListAsync(appSpec);

            Assert.IsTrue(applications.Any());
        }

        [Test]
        public async Task GetApplicationsByNameAndCodeTest()
        {

            var appSpec = new ApplicationSpecification();
            appSpec.Name = "MyApp1";
            appSpec.Code = "ap1";
            var repo = new EfRepository<Application>(this.DbContext);
            var applications = await repo.ListAsync(appSpec);

            Assert.IsTrue(applications.Any());
        }


        [Test]
        public async Task GetServersByGroupsTest()
        {
            var groupName = "windows";

            // Find all Allowed groups and child groups
            var repo = new GroupRepository(this.DbContext);

            var allGroups = await repo.ListAllAsync();

            var x = allGroups.Single(g => g.Name == "windows");
            var y = x.FlattenChildrends();

            var childGroups = repo.GetChildrenGroups(groupName);
            var allGroupNames = childGroups.Select(g => g.Name).ToArray();

            // Find all servers for these groups
            var repoSrv = new EfRepository<Server>(this.DbContext);
            //var servers = await repoSrv.ListAsync(new ServerWithGroupsSpecification(allGroupNames));
            var servers = await repoSrv.ListAsync(new ServerSpecification(allGroupNames,"poc"));

            foreach(Server srv in servers)
            {
                foreach(ServerGroup sg in srv.ServerGroups)
                {
                    var parentGroups = sg.Group.TraverseParents();

                }
            }

            Assert.IsTrue(childGroups.Any());

        }




    }

}