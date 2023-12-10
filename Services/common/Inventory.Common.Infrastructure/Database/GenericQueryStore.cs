using Ardalis.Specification;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Inventory.Common.Infrastructure.Database
{
    public class GenericQueryStore<TDbContext, T> : IGenericQueryStore<T>, IAsyncDisposable, IDisposable where T : Entity where TDbContext : DbContext
    {
        protected readonly TDbContext _dbContext;
        protected readonly IDbContextFactory<TDbContext> _dbContextFactory;
        protected readonly IMapper _mapper;

        public GenericQueryStore(TDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public GenericQueryStore(IDbContextFactory<TDbContext> dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = dbContextFactory.CreateDbContext();
            _mapper = mapper;
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


        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>()
                                    .AsNoTracking()
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();
        }

        public Task<TDtoEntity> GetByIdAsync<TDtoEntity>(int id) where TDtoEntity : class
        {
            return _dbContext.Set<T>()
                              .AsNoTracking()
                              .Where(x => x.Id == id)
                              .ProjectTo<TDtoEntity>(_mapper.ConfigurationProvider)
                              .FirstOrDefaultAsync();
        }


        public async Task<T> FirstOrDefaultAsync(IExpressionFilter<T> criteria = null,
                                                CancellationToken cancellationToken = default,
                                                params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>()
                               .AsNoTracking();

            if (criteria is not null && criteria.Predicate is not null)
            {
                query = query.Where(criteria.Predicate);
            }

            if (includes != null)
            {
                query = includes.Aggregate(query,
                                             (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TDtoEntity> FirstOrDefaultAsync<TDtoEntity>(IExpressionFilter<T> criteria = null,
                                                                      CancellationToken cancellationToken = default,
                                                                      params Expression<Func<T, object>>[] includes) where TDtoEntity : class
        {
            var query = _dbContext.Set<T>()
                               .AsNoTracking();

            if (criteria is not null && criteria.Predicate is not null)
            {
                query = query.Where(criteria.Predicate);
            }

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.ProjectTo<TDtoEntity>(_mapper.ConfigurationProvider)
                               .FirstOrDefaultAsync(cancellationToken);
        }


        public Task<IEnumerable<TDtoEntity>> ListAllAsync<TDtoEntity>(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                      int? offset = null, int? limit = null,
                                                                      CancellationToken cancellationToken = default,
                                                                      params Expression<Func<T, object>>[] includes) where TDtoEntity : class
        {
            return GetByCriteriaAsync<TDtoEntity>(criteria: null,
                                                  orderBy: orderBy,
                                                  offset: offset,
                                                  limit: limit,
                                                  cancellationToken: cancellationToken,
                                                  includes: includes);
        }

        public Task<IEnumerable<T>> ListAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                 int? offset = null, int? limit = null,
                                                 CancellationToken cancellationToken = default,
                                                 params Expression<Func<T, object>>[] includes)
        {
            return GetByCriteriaAsync(criteria: null,
                                      orderBy: orderBy,
                                      offset: offset,
                                      limit: limit,
                                      cancellationToken: cancellationToken,
                                      includes: includes);
        }

        public async Task<IEnumerable<TDtoEntity>> GetByCriteriaAsync<TDtoEntity>(IExpressionFilter<T> criteria = null,
                                                                                  Expression<Func<T, IEnumerable<TDtoEntity>>> ChildProjection = null,
                                                                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                                  int? offset = null, int? limit = null,
                                                                                  CancellationToken cancellationToken = default,
                                                                                  params Expression<Func<T, object>>[] includes) where TDtoEntity : class
        {
            var query = _dbContext.Set<T>()
                                  .AsNoTracking();

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

            // return the child projection if it is not null
            if (ChildProjection != null)
            {
                return await query.SelectMany<T, TDtoEntity>(ChildProjection).ToListAsync();
            }

            // return the default query projection
            return await query.ProjectTo<TDtoEntity>(_mapper.ConfigurationProvider)
                        .ToListAsync();

        }

        /// <summary>
        /// Retrieves a list of entities based on the specified criteria, ordering, offset, and limit.
        /// </summary>
        /// <param name="criteria">The expression filter criteria.</param>
        /// <param name="orderBy">The ordering function.</param>
        /// <param name="offset">The number of items to skip.</param>
        /// <param name="limit">The maximum number of items to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="includes">The related entities to include in the query.</param>
        /// <returns>The list of entities.</returns>
        /// <summary>
        public async Task<IEnumerable<T>> GetByCriteriaAsync(IExpressionFilter<T> criteria = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? offset = null, int? limit = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>()
                                  .AsNoTracking();

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

        /// Retrieves a queryable object based on the specified criteria, ordering, offset, and limit.
        /// </summary>
        /// <param name="criteria">The expression filter criteria.</param>
        /// <param name="orderBy">The ordering function.</param>
        /// <param name="offset">The number of items to skip.</param>
        /// <param name="limit">The maximum number of items to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="includes">The related entities to include in the query.</param>
        /// <returns>The queryable object.</returns>
        public IQueryable<T> GetQuery(IExpressionFilter<T> criteria = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? offset = null, int? limit = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>()
                                  .AsNoTracking();

            if (criteria is not null && criteria.Predicate is not null)
            {
                query = query.Where(criteria.Predicate);
            }

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
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

            return query;
        }
    }
}
