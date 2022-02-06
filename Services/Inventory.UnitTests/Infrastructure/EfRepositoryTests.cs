using Inventory.Domain;
using Inventory.Domain.Models;
using Inventory.Domain.Models.ManagedEntities;
using Inventory.Domain.Specifications;
using Inventory.Infrastructure.Databases;
using Inventory.Infrastructure.Databases.Repositories;
using Environment = Inventory.Domain.Models.Configuration.Environment;
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
using Inventory.Domain.Models.Credentials;
using Inventory.Domain.Models.Configuration;
using OperatingSystem = Inventory.Domain.Models.Configuration.OperatingSystem;

namespace Inventory.UnitTests
{
    public class EfRepositoryTests : BaseDbInventoryTests
    {

        [Test]
        public async Task Should_Query_All_Environments()
        {

            var repo = this.GetAsyncRepository<Environment>();
            var results = await repo.ListAsync();

            Assert.IsTrue(results.Any());
        }

        [Test]
        public async Task Should_Query_All_Credentials()
        {

            var repo = this.GetAsyncRepository<Credential>();
            var results = await repo.ListAsync();

            Assert.IsTrue(results.Any());
        }


        [Test]
        public async Task Should_Query_All_Locations()
        {

            var repo = this.GetAsyncRepository<Location>();
            var results = await repo.ListAsync();

            Assert.IsTrue(results.Any());
        }


        [Test]
        public async Task Should_Query_All_DataCenters()
        {

            var repo = this.GetAsyncRepository<DataCenter>();
            var results = await repo.ListAsync();

            Assert.IsTrue(results.Any());
        }

        [Test]
        public async Task Should_Query_All_OperatingSystems()
        {

            var repo = this.GetAsyncRepository<OperatingSystem>();
            var results = await repo.ListAsync();

            Assert.IsTrue(results.Any());
        }

        [Test]
        public async Task Should_Query_All_TrustLevels()
        {

            var repo = this.GetAsyncRepository<TrustLevel>();
            var results = await repo.ListAsync();

            Assert.IsTrue(results.Any());
        }


    }

}