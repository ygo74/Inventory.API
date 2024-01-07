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


        /// <summary>
        /// Asynchronously Get entity by id
        /// </summary>
        /// <param name="id">Entity's id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains the element with requested id />.
        /// </returns>
        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>()
                                    .AsNoTracking()
                                    .Where(x => x.Id == id)
                                    .FirstAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously Get entity by Id with projection
        /// </summary>
        /// <typeparam name="TDtoEntity"></typeparam>
        /// <param name="id">Entity's id</param>
        /// <param name="Projection">Query projection</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains the element with requested id />.
        /// </returns>
        public Task<TDtoEntity> GetByIdAsync<TDtoEntity>(int id, 
                                                        Expression<Func<T, TDtoEntity>> Projection = null,
                                                        CancellationToken cancellationToken = default) where TDtoEntity : class
        {

            var query = _dbContext.Set<T>()
                               .AsNoTracking()
                               .Where(x => x.Id == id);

            if (Projection != null)
            {
                // return custom projection
                return query.Select<T, TDtoEntity>(Projection).FirstOrDefaultAsync(cancellationToken);
            }

            // return default mapping from AutoMapper
            return query.ProjectTo<TDtoEntity>(_mapper.ConfigurationProvider)
                        .FirstAsync(cancellationToken);
        }


        /// <summary>
        /// Asynchronously Get first or default entity according the criteria
        /// </summary>
        /// <param name="criteria">Query's criteria</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="includes">Additional includes</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains <see langword="default" /> ( <typeparamref name="TSource" /> ) if
        ///     <paramref name="source" /> is empty; otherwise, the first element in <paramref name="source" />.
        /// </returns>
        public Task<T> FirstOrDefaultAsync(IExpressionFilter<T> criteria = null,
                                                CancellationToken cancellationToken = default,
                                                params Expression<Func<T, object>>[] includes)
        {

            // get query
            var query = GetQuery(
                criteria: criteria,
                includes: includes
            );

            return query.FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously Get first or default entity with projection according the criteria
        /// </summary>
        /// <param name="criteria">Query's criteria</param>
        /// <param name="Projection">Query projection</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="includes">Additional includes</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains <see langword="default" /> ( <typeparamref name="TSource" /> ) if
        ///     <paramref name="source" /> is empty; otherwise, the first element in <paramref name="source" />.
        /// </returns>
        public Task<TDtoEntity> FirstOrDefaultAsync<TDtoEntity>(IExpressionFilter<T> criteria = null,
                                                                      Expression<Func<T, TDtoEntity>> Projection = null,
                                                                      CancellationToken cancellationToken = default,
                                                                      params Expression<Func<T, object>>[] includes) where TDtoEntity : class
        {

            // get query
            var query = GetQuery(
                criteria: criteria,
                includes: includes
            );

            if (Projection != null)
            {
                return query.Select<T, TDtoEntity>(Projection).FirstOrDefaultAsync(cancellationToken);
            }

            return query.ProjectTo<TDtoEntity>(_mapper.ConfigurationProvider)
                               .FirstOrDefaultAsync(cancellationToken);
        }


        /// <summary>
        /// Asynchronously List all entities with Projection
        /// </summary>
        /// <typeparam name="TDtoEntity"></typeparam>
        /// <param name="orderBy">The ordering function.</param>
        /// <param name="Projection">Entity's projection</param>
        /// <param name="offset">The number of items to skip.</param>
        /// <param name="limit">The maximum number of items to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="includes">Additional includes</param>
        /// <returns>The list of entities.</returns>
        public Task<List<TDtoEntity>> ListAllAsync<TDtoEntity>(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                      Expression<Func<T, TDtoEntity>> Projection = null,
                                                                      int? offset = null, int? limit = null,
                                                                      CancellationToken cancellationToken = default,
                                                                      params Expression<Func<T, object>>[] includes) where TDtoEntity : class
        {
            return GetByCriteriaAsync<TDtoEntity>(criteria: null,
                                                  orderBy: orderBy,
                                                  Projection: Projection,
                                                  offset: offset,
                                                  limit: limit,
                                                  cancellationToken: cancellationToken,
                                                  includes: includes);
        }

        /// <summary>
        /// Asynchronously List all entities with SelectMany projection
        /// </summary>
        /// <typeparam name="TDtoEntity"></typeparam>
        /// <param name="orderBy">The ordering function.</param>
        /// <param name="Projection">SelectMany entity's projection</param>
        /// <param name="offset">The number of items to skip.</param>
        /// <param name="limit">The maximum number of items to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="includes">Additional includes</param>
        /// <returns>The list of entities.</returns>
        public Task<List<TDtoEntity>> ListAllWithManyAsync<TDtoEntity>(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                      Expression<Func<T, IEnumerable<TDtoEntity>>> Projection = null,
                                                                      int? offset = null, int? limit = null,
                                                                      CancellationToken cancellationToken = default,
                                                                      params Expression<Func<T, object>>[] includes) where TDtoEntity : class
        {
            return GetByCriteriaWithManySelectAsync<TDtoEntity>(criteria: null,
                                                  orderBy: orderBy,
                                                  Projection: Projection,
                                                  offset: offset,
                                                  limit: limit,
                                                  cancellationToken: cancellationToken,
                                                  includes: includes);
        }

        /// <summary>
        /// Asynchronously List all entities
        /// </summary>
        /// <param name="orderBy">The ordering function.</param>
        /// <param name="offset">The number of items to skip.</param>
        /// <param name="limit">The maximum number of items to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="includes">Additional includes</param>
        /// <returns>The list of entities.</returns>
        public Task<List<T>> ListAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
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

        /// <summary>
        /// Asynchronously Get entities by criteria with Projection
        /// </summary>
        /// <typeparam name="TDtoEntity"></typeparam>
        /// <param name="criteria">The expression filter criteria.</param>
        /// <param name="Projection">Entity's projection</param>
        /// <param name="orderBy">The ordering function.</param>
        /// <param name="offset">The number of items to skip.</param>
        /// <param name="limit">The maximum number of items to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="includes">Additional includes</param>
        /// <returns>The list of entities.</returns>
        public Task<List<TDtoEntity>> GetByCriteriaAsync<TDtoEntity>(IExpressionFilter<T> criteria = null,
                                                                                  Expression<Func<T, TDtoEntity>> Projection = null,
                                                                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                                  int? offset = null, int? limit = null,
                                                                                  CancellationToken cancellationToken = default,
                                                                                  params Expression<Func<T, object>>[] includes) where TDtoEntity : class
        {

            // get query
            var query = GetQuery(
                criteria: criteria,
                orderBy: orderBy,
                offset: offset,
                limit: limit,
                includes: includes
            );

            // return the child projection if it is not null
            if (Projection != null)
            {
                return query.Select<T, TDtoEntity>(Projection).ToListAsync(cancellationToken);
            }

            // return the default query projection
            return query.ProjectTo<TDtoEntity>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);

        }

        /// <summary>
        /// Asynchronously Get entities by criteria with SelectMany projection
        /// </summary>
        /// <typeparam name="TDtoEntity"></typeparam>
        /// <param name="criteria">The expression filter criteria.</param>
        /// <param name="Projection">SelectMany entity's projection</param>
        /// <param name="orderBy">The ordering function.</param>
        /// <param name="offset">The number of items to skip.</param>
        /// <param name="limit">The maximum number of items to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="includes">Additional includes</param>
        /// <returns>The list of entities.</returns>
        public Task<List<TDtoEntity>> GetByCriteriaWithManySelectAsync<TDtoEntity>(IExpressionFilter<T> criteria = null,
                                                                                  Expression<Func<T, IEnumerable<TDtoEntity>>> Projection = null,
                                                                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                                  int? offset = null, int? limit = null,
                                                                                  CancellationToken cancellationToken = default,
                                                                                  params Expression<Func<T, object>>[] includes) where TDtoEntity : class
        {

            // get query
            var query = GetQuery(
                criteria: criteria,
                orderBy: orderBy,
                offset: offset,
                limit: limit,
                includes: includes
            );

            // return the child projection if it is not null
            if (Projection != null)
            {
                return query.SelectMany<T, TDtoEntity>(Projection).ToListAsync(cancellationToken);
            }

            // return the default query projection
            return query.ProjectTo<TDtoEntity>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);

        }

        /// <summary>
        /// Asynchronously Retrieves a list of entities based on the specified criteria, ordering, offset, and limit.
        /// </summary>
        /// <param name="criteria">The expression filter criteria.</param>
        /// <param name="orderBy">The ordering function.</param>
        /// <param name="offset">The number of items to skip.</param>
        /// <param name="limit">The maximum number of items to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="includes">The related entities to include in the query.</param>
        /// <returns>The list of entities.</returns>
        /// <summary>
        public Task<List<T>> GetByCriteriaAsync(IExpressionFilter<T> criteria = null,
                                                             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                             int? offset = null, int? limit = null,
                                                             CancellationToken cancellationToken = default,
                                                             params Expression<Func<T, object>>[] includes)
        {
            // get query
            var query = GetQuery(
                criteria: criteria,
                orderBy: orderBy,
                offset: offset,
                limit: limit,
                includes: includes
            );

            return query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a queryable object based on the specified criteria, ordering, offset, and limit.
        /// </summary>
        /// <param name="criteria">The expression filter criteria.</param>
        /// <param name="orderBy">The ordering function.</param>
        /// <param name="offset">The number of items to skip.</param>
        /// <param name="limit">The maximum number of items to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="includes">The related entities to include in the query.</param>
        /// <returns>The queryable object.</returns>
        public IQueryable<T> GetQuery(IExpressionFilter<T> criteria = null, 
                                      Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
                                      int? offset = null, int? limit = null, 
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

        /// <summary>
        ///     Asynchronously returns the number of elements in a sequence.
        /// </summary>
        /// <param name="criteria">The expression filter criteria.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains the number of elements in the input sequence.
        /// </returns>
        public Task<int> CountAsync(IExpressionFilter<T> criteria = null, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>()
                                  .AsNoTracking();

            if (criteria is not null && criteria.Predicate is not null)
            {
                query = query.Where(criteria.Predicate);
            }

            return query.CountAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously determines whether a sequence contains any elements.
        /// </summary>
        /// <param name="criteria">The expression filter criteria.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains <see langword="true" /> if the source sequence contains any elements; otherwise, <see langword="false" />.
        /// </returns>
        public Task<bool> AnyAsync(IExpressionFilter<T> criteria = null, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>()
                                  .AsNoTracking();

            if (criteria is not null && criteria.Predicate is not null)
            {
                query = query.Where(criteria.Predicate);
            }

            return query.AnyAsync(cancellationToken);
        }

    }
}
