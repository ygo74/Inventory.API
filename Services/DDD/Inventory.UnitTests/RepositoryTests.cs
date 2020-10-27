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
    public class RepositoryTests
    {
        //private readonly DbContextOptions<InventoryDbContext> _dbOptions = new DbContextOptionsBuilder<InventoryDbContext>()
        //                                                                    .UseInMemoryDatabase(databaseName: "in-memory")
        //                                                                    .Options;

        private readonly DbContextOptions<InventoryDbContext> _dbOptions = new DbContextOptionsBuilder<InventoryDbContext>()
                                                                            .UseNpgsql("host=localhost;port=55432;database=blogdb;username=bloguser;password=bloguser")
                                                                            .Options;


        private InventoryDbContext _dbContext;



        //private List<Domain.Models.OperatingSystem> GetOperatingSystems()
        //{
        //    return new List<Domain.Models.OperatingSystem>()
        //    {
        //        new Domain.Models.OperatingSystem()
        //        {
        //            Name = "Windows 2008 R2",
        //            Familly = OsFamilly.Windows
        //        },
        //        new Domain.Models.OperatingSystem()
        //        {
        //            Name = "Windows 2012 R2",
        //            Familly = OsFamilly.Windows
        //        },
        //        new Domain.Models.OperatingSystem()
        //        {
        //            Name = "Windows 2016",
        //            Familly = OsFamilly.Windows
        //        },
        //        new Domain.Models.OperatingSystem()
        //        {
        //            Name = "Windows 2019",
        //            Familly = OsFamilly.Windows
        //        },
        //        new Domain.Models.OperatingSystem()
        //        {
        //            Name = "RHEL 7",
        //            Familly = OsFamilly.Linux
        //        },
        //        new Domain.Models.OperatingSystem()
        //        {
        //            Name = "RHEL 8",
        //            Familly = OsFamilly.Linux
        //        }
        //    };
        //}




        //private List<Server> GetFakeServers(List<Domain.Models.OperatingSystem> operatingSystems)
        //{
        //    var windows2019 = operatingSystems.Single(os => os.Name.Equals("Windows 2019", StringComparison.OrdinalIgnoreCase));
        //    var windows2016 = operatingSystems.Single(os => os.Name.Equals("Windows 2016", StringComparison.OrdinalIgnoreCase));
        //    var rhel7 = operatingSystems.Single(os => os.Name.Equals("RHEL 7", StringComparison.OrdinalIgnoreCase));
        //    var rhel8 = operatingSystems.Single(os => os.Name.Equals("RHEL 8", StringComparison.OrdinalIgnoreCase));

        //    return new List<Server>()
        //    {
        //        new Server("msTest1", windows2019),
        //        new Server("msTest2", windows2019),
        //        new Server("msTest3", windows2016),
        //        new Server("msTest4", windows2016),
        //        new Server("lxTest1", rhel7),
        //        new Server("lxTest2", rhel7),
        //        new Server("lxTest3", rhel8),
        //        new Server("lxTest4", rhel8)
        //    };
        //}

        //private List<Group> GetReferentialGroups(List<Domain.Models.OperatingSystem> operatingSystems)
        //{

        //    //OS Groups
        //    var osGroups = new Group(name: "OperatingSystems", ansibleGroupName: "os");

        //    var windowsGroups = new Group(name: "Windows");
        //    var linuxGroups = new Group(name: "Linux");

        //    osGroups.AddSubGroups(windowsGroups);
        //    osGroups.AddSubGroups(linuxGroups);

        //    windowsGroups.AddSubGroups(new Group("Windows 2019", "system_windows_2k19"));
        //    windowsGroups.AddSubGroups(new Group("Windows 2016", "system_windows_2k16"));

        //    linuxGroups.AddSubGroups(new Group("RHEL 7", "system_rhel_7"));
        //    linuxGroups.AddSubGroups(new Group("RHEL 8", "system_rhel_8"));

        //    var allgroups = new List<Group>()
        //    {
        //        osGroups,
        //        windowsGroups,
        //        linuxGroups
        //    };
        //    allgroups.AddRange(windowsGroups.Children);
        //    allgroups.AddRange(linuxGroups.Children);

        //    return allgroups;

        //}


        [SetUp]
        public void Setup()
        {

            PostgresqlDocker docker = new PostgresqlDocker();
            docker.Start().Wait();

            _dbContext = new InventoryDbContext(_dbOptions);
            _dbContext.Database.EnsureCreated();
            _dbContext.Database.Migrate();

            //if (!_dbContext.Groups.Any())
            //{

            //    var operatingSystems = this.GetOperatingSystems();
            //    var servers = this.GetFakeServers(operatingSystems);
            //    var groups = this.GetReferentialGroups(operatingSystems);


            //    _dbContext.OperatingSystems.AddRange(operatingSystems);
            //    _dbContext.Groups.AddRange(groups);
            //    _dbContext.Servers.AddRange(servers);

            //    _dbContext.SaveChanges();

            //}
        }

        [Test]
        public async Task GetServerByIdAsyncTest()
        {

            var repo = new ServerRepository(_dbContext);
            var server = await repo.GetByIdAsync(1);

            //Debug.WriteLine(server.Variables.RootElement);

            Assert.AreEqual(1, server.ServerId);
        }

        [Test]
        public void GetServerByIdTest()
        {

            var repo = new ServerRepository(_dbContext);
            var server = repo.GetFirstOrDefault(
                x => x.ServerId == 1,
                null,
                include: x => x.Include(s => s.ServerGroups).ThenInclude(g => g.Group).ThenInclude(g => g.Parent));

            Assert.AreEqual(1, server.ServerId);
        }

        [Test]
        public void GetAllGroups()
        {

            var repo = new GroupRepository(_dbContext);
            //var group = repo.GetAll().Include(g => g.Parent).ThenInclude(g => g.ServerGroups).ThenInclude(sg => sg.Server).AsEnumerable().Where(g => g.Parent == null).ToList();
            //var group = _dbContext.Groups.Include(g => g.Parent).ThenInclude(g => g.ServerGroups).ThenInclude(sg => sg.Server).Where(g => g.Name == "linux").ToLookup(g => g.Name, g=> g.ServerGroups);
            var group = _dbContext.Groups.Include(g => g.Parent).Single(g => g.Name == "Windows 2016");
            var allGroups = group.TraverseParents().ToList();

            Assert.AreEqual(3, allGroups.Count());
        }

    }

    public static class FlattenExtension
    {
        public static IEnumerable<Group> TraverseParents(this Group entity)
        {
            var stack = new Stack<Group>();
            stack.Push(entity);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;
                if (null != current.Parent)
                {
                    stack.Push(current.Parent);
                }

            }

        }
    }
}