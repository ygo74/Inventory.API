using Bogus;
using Inventory.Domain;
using Inventory.Domain.Models;
using Inventory.Infrastructure.Databases;
using Inventory.Infrastructure.Databases.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Npgsql.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Inventory.UnitTests
{
    public class BaseModelInventoryTests
    { 

        public BaseModelInventoryTests()
        {

        }

        [Test]
        public void TestBogus()
        {

            var TestApplications = new Faker<Application>()
                .RuleFor(app => app.ApplicationId, f => f.IndexFaker)
                .RuleFor(app => app.Name, f => f.Lorem.Sentence(10))
                .RuleFor(app => app.Code, f => f.Lorem.Word());

            var myApp = TestApplications.Generate(10);

            Assert.IsTrue(myApp.Count == 10);

        }

    }

}