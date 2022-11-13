using Inventory.Domain.Base.Repository;
using Inventory.Infrastructure.Base.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Networks.Infrastructure
{
    public class NetworksRepository<T> : EfAsyncRepository<NetworksDbContext, T> where T : class
    {
        public NetworksRepository(NetworksDbContext dbContext) : base(dbContext)
        {

        }

        public NetworksRepository(IDbContextFactory<NetworksDbContext> dbContextFactory) : base(dbContextFactory)
        {

        }

    }
}
