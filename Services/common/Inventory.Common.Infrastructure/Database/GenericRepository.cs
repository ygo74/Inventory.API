using Ardalis.Specification;
using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Models;
using Inventory.Common.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Common.Infrastructure.Database
{
    public class GenericRepository<TDbContext, T> : IAsyncRepository<T>, IAsyncDisposable, IDisposable where TDbContext : DbContext, IUnitOfWork where T : Entity
    {
        protected readonly TDbContext _dbContext;
        protected readonly IDbContextFactory<TDbContext> _dbContextFactory;

        public GenericRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GenericRepository(IDbContextFactory<TDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public TDbContext DbContext
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

        #region IDisposable Support
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

        #endregion

        public virtual async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.Where(x => x.Id == id)
                              .FirstOrDefaultAsync(cancellationToken);

        }

        public virtual Task<T> FirstAsync(IExpressionFilter<T> criteria = null, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (criteria is not null && criteria.Predicate is not null)
            {
                query = query.Where(criteria.Predicate);
            }

            return query.FirstAsync(cancellationToken);
        }

        public virtual Task<T> FirstOrDefaultAsync(IExpressionFilter<T> criteria = null, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (criteria is not null && criteria.Predicate is not null)
            {
                query = query.Where(criteria.Predicate);
            }

            return query.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual Task<IEnumerable<T>> ListAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? offset = null, int? limit = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
        {
            return GetByCriteriaAsync(criteria: null,
                                      orderBy: orderBy,
                                      offset: offset,
                                      limit: limit,
                                      cancellationToken: cancellationToken,
                                      includes: includes);
        }

        public virtual async Task<IEnumerable<T>> GetByCriteriaAsync(IExpressionFilter<T> criteria = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? offset = null, int? limit = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (criteria is not null && criteria.Predicate is not null)
            {
                query = query.Where(criteria.Predicate);
            }

            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (offset.HasValue)
            {
                query = query.Skip(offset.Value);
            }

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<(T result, int nbchanges)> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Add(entity);

            var nbChanges = await SaveChangesAsync(cancellationToken);

            return (entity, nbChanges);
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().AddRange(entities);

            await SaveChangesAsync(cancellationToken);

            return entities;
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Update(entity);

            await SaveChangesAsync(cancellationToken);

        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().UpdateRange(entities);

            await SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Remove(entity);

            await SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().RemoveRange(entities);

            await SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }


    }
}
