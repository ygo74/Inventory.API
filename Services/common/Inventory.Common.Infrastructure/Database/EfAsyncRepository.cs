using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Inventory.Common.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Infrastructure.Database
{
    public class EfAsyncRepository<DB,T> : RepositoryBase<T>, IAsyncRepositoryWithSpecification<T>, IAsyncDisposable, IDisposable where DB : DbContext,IUnitOfWork where T : class
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

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext?.Dispose();
            }
        }
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(disposing: false);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
            GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_dbContext is not null)
            {
                await _dbContext.DisposeAsync().ConfigureAwait(false);
            }
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
