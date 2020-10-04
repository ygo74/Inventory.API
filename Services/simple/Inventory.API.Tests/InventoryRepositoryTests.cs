using Inventory.Infrastructure;
using Inventory.Domain.Models;
using Inventory.API.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;

namespace Inventory.API.Tests
{
    public class InventoryRepositoryTests
    {
        private InventoryDbContext _dbContext;

        private InventoryRepository _inventoryRepository;


        [SetUp]
        public void Setup()
        {

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<InventoryDbContext>().UseSqlite(connection).Options;

            _dbContext = new InventoryDbContext(options);
            _dbContext.Database.EnsureCreated();
            _inventoryRepository = new InventoryRepository(_dbContext);

            this.SeedDatabase();
        }

        [TearDown]
        public void Dispose()
        {
            _dbContext.Dispose();
        }


        private void SeedDatabase()
        {
            _inventoryRepository.CreateGroup("OperatingSystems");
            _inventoryRepository.CreateGroup("Locations");
            _inventoryRepository.CreateGroup("Applications");

            _inventoryRepository.CreateGroup("Windows", "OperatingSystems");
            _inventoryRepository.CreateGroup("Linux", "OperatingSystems");

            _inventoryRepository.CreateGroup("Windows 2016", "Windows");
            _inventoryRepository.CreateGroup("Windows 2019", "Windows");

            _inventoryRepository.CreateGroup("RHEL 7", "Linux");
            _inventoryRepository.CreateGroup("RHEL 8", "Linux");
            _inventoryRepository.SaveChangesAsync().Wait();

        }



        [Test]
        public void Create_Group_Without_Parent_Success()
        {
            var newGroup = _inventoryRepository.CreateGroup("TopGroup");
            _inventoryRepository.SaveChangesAsync().Wait();
            Assert.IsNotNull(newGroup);
        }

        [Test]
        public void Create_Group_With_Non_Existing_Parent_Failure()
        {
            Assert.Throws< ArgumentException>(delegate()
            {
                var newGroup = _inventoryRepository.CreateGroup("SecondGroup", "TopGroupNonExisting");
                _inventoryRepository.SaveChangesAsync().Wait();
            });
        }

        [Test]
        public void Create_Group_With_Existing_Parent_Sucess()
        {
            _inventoryRepository.CreateGroup("TopGroup");
            var newSecondGroup = _inventoryRepository.CreateGroup("SecongGroup", "TopGroup");
            _inventoryRepository.SaveChangesAsync().Wait();
            Assert.IsNotNull(newSecondGroup);
        }

        [Test]
        public async Task Can_Retrieve_Existing_Group()
        {

            var parents = await _inventoryRepository.GetParentGroups(4);
            var childrens = await _inventoryRepository.GetChildrenGroups(4);

            var all = parents.Concat(childrens).Distinct().ToList();

            var allgroups = _dbContext.Groups.Where(grp => grp.Name == "windows 2019")
                .Include(grp => grp.Parent).ToList();


            var group = await _inventoryRepository.GetGroupById(1);
            Assert.IsNotNull(group);
        }




    }
}
