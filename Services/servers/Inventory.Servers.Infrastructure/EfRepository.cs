using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Inventory.Domain.Base.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Inventory.Servers.Infrastructure
{
    /// <summary>
    /// "There's some repetition here - couldn't we have some the sync methods call the async?"
    /// https://blogs.msdn.microsoft.com/pfxteam/2012/04/13/should-i-expose-synchronous-wrappers-for-asynchronous-methods/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EfRepository<T> : RepositoryBase<T> , IAsyncRepository<T>, IAsyncDisposable where T : class
    {
        protected readonly ServerDbContext _dbContext;
        protected readonly IDbContextFactory<ServerDbContext> _dbContextFactory;

        public EfRepository(ServerDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public EfRepository(IDbContextFactory<ServerDbContext> dbContextFactory) : base(dbContextFactory.CreateDbContext())
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public ServerDbContext DbContext
        {
            get { return _dbContext;  }
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _dbContext;
            }
        }

        public ValueTask DisposeAsync()
        {
            if (_dbContextFactory != null)
            {
                return _dbContext.DisposeAsync();
            }
            return new ValueTask(Task.CompletedTask);
        }

        public async Task<T> FirstAsync(ISpecification<T> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstAsync();
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstOrDefaultAsync();
        }

    }
}