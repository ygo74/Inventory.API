using Inventory.Domain;
using Inventory.Domain.Models;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.UnitTests
{
    public class RepositoryTests
    {
        private readonly DbContextOptions<InventoryDbContext> _dbOptions = new DbContextOptionsBuilder<InventoryDbContext>()
                                                                            .UseInMemoryDatabase(databaseName: "in-memory")
                                                                            .Options;

        private InventoryDbContext _dbContext;




        private List<Server> GetFakeServers()
        {
            return new List<Server>()
            {
                new Server()
                {
                    Name   = "msTest1",
                    OperatingSystem = OsType.Windows
                },
                new Server()
                {
                    Name   = "msTest2",
                    OperatingSystem = OsType.Windows
                },
                new Server()
                {
                    Name   = "lxTest3",
                    OperatingSystem = OsType.Linux
                },
                new Server()
                {
                    Name   = "lxTest4",
                    OperatingSystem = OsType.Linux
                }
            };
        }

        private List<Group> GetFakeGroups()
        {

            var topGroups = new List<Group>()
            {
                new Group() {
                    Name = "operating_system"
                },
                new Group() {
                    Name = "location"
                },
                new Group() {
                    Name = "applications"
                }
            };

            var OsGroups = new List<Group>()
            {
                new Group() {
                    Name = "windows",
                    Parent = topGroups[0]
                },
                new Group() {
                    Name = "linux",
                    Parent = topGroups[0]
                }
            };


            return new List<Group>().Concat(topGroups).Concat(OsGroups).ToList();

        }


        [SetUp]
        public void Setup()
        {
            _dbContext = new InventoryDbContext(_dbOptions);

            var servers = this.GetFakeServers();
            var groups = this.GetFakeGroups();

            foreach (Server srv in servers)
            {
                srv.ServerGroups = new List<ServerGroup>()
                {
                    new ServerGroup()
                    {
                        Server = srv,
                        Group  = groups.Where(g => g.Name == "operating_system").FirstOrDefault()
                    },
                    new ServerGroup()
                    {
                        Server = srv,
                        Group  = groups.Where(g => g.Name == srv.OperatingSystem.ToString().ToLower()).FirstOrDefault()
                    }
                    //new ServerGroup()
                    //{
                    //    Server = srv,
                    //    Group  = groups.Where(g => g.Name == "location").FirstOrDefault()
                    //},
                    //new ServerGroup()
                    //{
                    //    Server = srv,
                    //    Group  = groups.Where(g => g.Name == "france").FirstOrDefault()
                    //},
                };

            }

            _dbContext.Groups.AddRange(groups);
            _dbContext.Servers.AddRange(servers);

            //_dbContext.Servers.AttachRange(servers);

            //_dbContext.AddRange(GetFakeServers());
            _dbContext.SaveChanges();

        }

        [Test]
        public async Task GetServerByIdAsyncTest()
        {

            var repo = new ServerRepository(_dbContext);
            var server = await repo.GetByIdAsync(3);

            Assert.AreEqual(3, server.ServerId);
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
            var group = _dbContext.Groups.Include(g => g.Parent).ThenInclude(g => g.ServerGroups).ThenInclude(sg => sg.Server).Where(g => g.Name == "linux").ToLookup(g => g.Name, g=> g.ServerGroups);

            Assert.AreEqual(1, group.Count());
        }


    }
}