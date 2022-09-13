using Inventory.Configuration.UnitTests.SeedWork;
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
            var dbContext = UnitTestsContext.Current.DbContext;

            dbContext.Datacenters.AddRange(DataCenterSeed.Get());

            dbContext.SaveChanges();

        }

    }
}
