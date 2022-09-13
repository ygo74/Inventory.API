using Inventory.Infrastructure.Base.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Infrastructure
{
    public class ConfigurationRepository<T> : EfAsyncRepository<ConfigurationDbContext, T> where T : class
    {
        public ConfigurationRepository(ConfigurationDbContext dbContext) :base(dbContext)
        {

        }

        public ConfigurationRepository(IDbContextFactory<ConfigurationDbContext> dbContextFactory) : base(dbContextFactory)
        {

        }

    }
}
