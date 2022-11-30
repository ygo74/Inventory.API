using Inventory.Configuration.Infrastructure;
using Inventory.Configuration.UnitTests.SeedWork;
using Inventory.Common.Domain.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests
{
    public abstract class DbUnitTests
    {

        protected DbUnitTests()
        {
            InitDatabase();
        }


        private void InitDatabase()
        {
            var dbContext = UnitTestsContext.Current.GetService<ConfigurationDbContext>();

            dbContext.Datacenters.AddRange(DataCenterSeed.Get());
            dbContext.Plugins.AddRange(PluginSeed.Get());
            dbContext.SaveChanges();

            dbContext.Credentials.AddRange(CredentialSeed.Get());

            dbContext.SaveChanges();

        }

    }
}
