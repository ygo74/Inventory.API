using Ardalis.Specification;
using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Common.Domain.Repository
{
    public interface IAsyncRepositoryWithSpecification<T> : IRepositoryBase<T> where T : class
    {
        IUnitOfWork UnitOfWork { get; }

        Task<T> FirstAsync(ISpecification<T> spec);
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
    }

    public interface IAsyncRepository<T> where T : Entity
    {


        IUnitOfWork UnitOfWork { get; }

        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);

        Task<T> FirstAsync(IExpressionFilter<T> criteria = null, CancellationToken cancellationToken = default);
        Task<T> FirstOrDefaultAsync(IExpressionFilter<T> criteria = null, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> ListAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          int? offset = null, int? limit = null,
                                          CancellationToken cancellationToken = default,
                                          params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<T>> GetByCriteriaAsync(IExpressionFilter<T> criteria = null,
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                int? offset = null, int? limit = null,
                                                CancellationToken cancellationToken = default,
                                                params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Adds an entity in the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="T" />.
        /// </returns>
        Task<int> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the given entities in the database
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="IEnumerable<T>" />.
        /// </returns>
        Task<int> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an entity in the database
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the given entities in the database
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<int> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an entity in the database
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the given entities in the database
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Persists changes to the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }

}
