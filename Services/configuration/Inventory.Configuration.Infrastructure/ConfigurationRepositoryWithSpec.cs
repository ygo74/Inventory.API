using AutoMapper;
using Inventory.Common.Domain.Models;
using Inventory.Common.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Infrastructure
{
    public class ConfigurationRepositoryWithSpec<T> : EfAsyncRepository<ConfigurationDbContext, T> where T : class
    {
        //public ConfigurationRepository(ConfigurationDbContext dbContext) :base(dbContext)
        //{

        //}

        public ConfigurationRepositoryWithSpec(IDbContextFactory<ConfigurationDbContext> dbContextFactory) : base(dbContextFactory)
        {

        }

    }

    public class ConfigurationRepository<T> : GenericRepository<ConfigurationDbContext, T> where T : Entity
    {
        //public ConfigurationRepository(ConfigurationDbContext dbContext) : base(dbContext)
        //{

        //}

        public ConfigurationRepository(IDbContextFactory<ConfigurationDbContext> dbContextFactory) : base(dbContextFactory)
        {

        }
    }


    public class ConfigurationQueryStore<T> : GenericQueryStore<ConfigurationDbContext, T> where T : Entity
    {
        //public ConfigurationQueryStore(ConfigurationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        //{

        //}

        public ConfigurationQueryStore(IDbContextFactory<ConfigurationDbContext> dbContextFactory, IMapper mapper) : base(dbContextFactory, mapper)
        {

        }
    }

}
