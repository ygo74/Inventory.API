using Inventory.Domain;
using Inventory.Domain.Models;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
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
                    Name   = "Test1",
                    Groups = new List<Group>()
                    {
                        new Group()
                        {
                            GroupId = 1,
                            Name = "Windows"
                        }
                    }
                    
                }
            };
        }


        [SetUp]
        public void Setup()
        {
            _dbContext = new InventoryDbContext(_dbOptions);
            _dbContext.AddRange(GetFakeServers());
            _dbContext.SaveChanges();

        }

        [Test]
        public async Task Test1()
        {

            var repo = new InventoryRepository(_dbContext);
            var logger = Mock.Of<ILogger<InventoryService>>();
            var service = new InventoryService(repo, logger);
            var server = await repo.GetAsync(1);

            Assert.AreEqual(1, server.Id);
        }
    }
}