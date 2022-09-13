using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Inventory.Domain.Base.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Base.Database
{
    public class EfAsyncRepository<DB,T> : RepositoryBase<T>, IAsyncRepository<T>, IAsyncDisposable where DB : DbContext,IUnitOfWork where T : class
    {
        protected readonly DB _dbContext;
        protected readonly IDbContextFactory<DB> _dbContextFactory;

        public EfAsyncRepository(DB dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public EfAsyncRepository(IDbContextFactory<DB> dbContextFactory) : base(dbContextFactory.CreateDbContext())
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public DB DbContext
        {
            get { return _dbContext; }
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
