using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Common.Domain.Repository
{
    public interface IGenericQueryStore<T> where T : Entity
    {
        /// <summary>
        /// Asynchronously Get entity by id
        /// </summary>
        /// <param name="id">Entity's id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains the element with requested id />.
        /// </returns>
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);

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
        Task<TDtoEntity> GetByIdAsync<TDtoEntity>(int id,
                                                  Expression<Func<T, TDtoEntity>> Projection = null,
                                                  CancellationToken cancellationToken = default) where TDtoEntity : class;

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
        Task<T> FirstOrDefaultAsync(IExpressionFilter<T> criteria = null,
                                                CancellationToken cancellationToken = default,
                                                params Expression<Func<T, object>>[] includes);

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
        Task<TDtoEntity> FirstOrDefaultAsync<TDtoEntity>(IExpressionFilter<T> criteria = null,
                                                         Expression<Func<T, TDtoEntity>> Projection = null,
                                                        CancellationToken cancellationToken = default,
                                                        params Expression<Func<T, object>>[] includes) where TDtoEntity : class;

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
        Task<List<TDtoEntity>> ListAllAsync<TDtoEntity>(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                               Expression<Func<T, TDtoEntity>> Projection = null,
                                                               int? offset = null, int? limit = null,
                                                               CancellationToken cancellationToken = default,
                                                               params Expression<Func<T, object>>[] includes) where TDtoEntity : class;


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
        Task<List<TDtoEntity>> ListAllWithManyAsync<TDtoEntity>(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                               Expression<Func<T, IEnumerable<TDtoEntity>>> Projection = null,
                                                               int? offset = null, int? limit = null,
                                                               CancellationToken cancellationToken = default,
                                                               params Expression<Func<T, object>>[] includes) where TDtoEntity : class;


        /// <summary>
        /// Asynchronously List all entities
        /// </summary>
        /// <param name="orderBy">The ordering function.</param>
        /// <param name="offset">The number of items to skip.</param>
        /// <param name="limit">The maximum number of items to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="includes">Additional includes</param>
        /// <returns>The list of entities.</returns>
        Task<List<T>> ListAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          int? offset = null, int? limit = null,
                                          CancellationToken cancellationToken = default,
                                          params Expression<Func<T, object>>[] includes);

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
        Task<List<TDtoEntity>> GetByCriteriaAsync<TDtoEntity>(IExpressionFilter<T> criteria = null,
                                                                     Expression<Func<T, TDtoEntity>> Projection = null,
                                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                     int? offset = null, int? limit = null,
                                                                     CancellationToken cancellationToken = default,
                                                                     params Expression<Func<T, object>>[] includes) where TDtoEntity : class;

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
        Task<List<TDtoEntity>> GetByCriteriaWithManySelectAsync<TDtoEntity>(IExpressionFilter<T> criteria = null,
                                                                     Expression<Func<T, IEnumerable<TDtoEntity>>> Projection = null,
                                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                     int? offset = null, int? limit = null,
                                                                     CancellationToken cancellationToken = default,
                                                                     params Expression<Func<T, object>>[] includes) where TDtoEntity : class;


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
        Task<List<T>> GetByCriteriaAsync(IExpressionFilter<T> criteria = null,
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                int? offset = null, int? limit = null,
                                                CancellationToken cancellationToken = default,
                                                params Expression<Func<T, object>>[] includes);

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
        IQueryable<T> GetQuery(IExpressionFilter<T> criteria = null,
                               Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                               int? offset = null, int? limit = null,
                               params Expression<Func<T, object>>[] includes);


        /// <summary>
        ///     Asynchronously returns the number of elements in a sequence.
        /// </summary>
        /// <param name="criteria">The expression filter criteria.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains the number of elements in the input sequence.
        /// </returns>
        Task<int> CountAsync(IExpressionFilter<T> criteria = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously determines whether a sequence contains any elements.
        /// </summary>
        /// <param name="criteria">The expression filter criteria.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains <see langword="true" /> if the source sequence contains any elements; otherwise, <see langword="false" />.
        /// </returns>
        Task<bool> AnyAsync(IExpressionFilter<T> criteria = null, CancellationToken cancellationToken = default);
    }

}
