using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Inventory.Domain.Base.Repository;
using Inventory.Infrastructure.Base.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Inventory.Devices.Infrastructure
{
    /// <summary>
    /// "There's some repetition here - couldn't we have some the sync methods call the async?"
    /// https://blogs.msdn.microsoft.com/pfxteam/2012/04/13/should-i-expose-synchronous-wrappers-for-asynchronous-methods/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DevicesRepository<T> : EfAsyncRepository<ServerDbContext, T> where T : class
    {
        public DevicesRepository(ServerDbContext dbContext) : base(dbContext)
        {

        }

        public DevicesRepository(IDbContextFactory<ServerDbContext> dbContextFactory) : base(dbContextFactory)
        {

        }
    }
}