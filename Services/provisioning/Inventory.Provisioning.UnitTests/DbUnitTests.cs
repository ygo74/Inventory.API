using Inventory.Provisioning.Domain.Models;
using Inventory.Provisioning.Infrastructure;
using Inventory.Provisioning.UnitTests.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Provisioning.UnitTests
{
    public abstract class DbUnitTests
    {

        protected DbUnitTests()
        {
            InitDatabase();
        }


        private void InitDatabase()
        {
            var dbContext = UnitTestsContext.Current.GetService<ProvisioningDbContext>();

            // No dependencies
            dbContext.LabelNames.AddRange(LabelNameSeed.Get());
            dbContext.SaveChanges();

            // dependencies
            dbContext.SaveChanges();

        }

    }
}
