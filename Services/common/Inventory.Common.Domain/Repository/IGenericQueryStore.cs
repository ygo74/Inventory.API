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
        Task<TDtoEntity> GetByIdAsync<TDtoEntity>(int id,
                                                  Expression<Func<T, TDtoEntity>> Projection = null,
                                                  CancellationToken cancellationToken = default) where TDtoEntity : class;
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<T> FirstOrDefaultAsync(IExpressionFilter<T> criteria = null,
                                                CancellationToken cancellationToken = default,
                                                params Expression<Func<T, object>>[] includes);

        Task<TDtoEntity> FirstOrDefaultAsync<TDtoEntity>(IExpressionFilter<T> criteria = null,
                                                         Expression<Func<T, TDtoEntity>> Projection = null,
                                                        CancellationToken cancellationToken = default,
                                                        params Expression<Func<T, object>>[] includes) where TDtoEntity : class;

        Task<IEnumerable<TDtoEntity>> ListAllAsync<TDtoEntity>(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                               Expression<Func<T, TDtoEntity>> Projection = null,
                                                               Expression<Func<T, IEnumerable<TDtoEntity>>> ManyProjection = null,
                                                               int? offset = null, int? limit = null,
                                                               CancellationToken cancellationToken = default,
                                                               params Expression<Func<T, object>>[] includes) where TDtoEntity : class;

        Task<IEnumerable<TDtoEntity>> ListAllWithManyAsync<TDtoEntity>(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                               Expression<Func<T, IEnumerable<TDtoEntity>>> Projection = null,
                                                               int? offset = null, int? limit = null,
                                                               CancellationToken cancellationToken = default,
                                                               params Expression<Func<T, object>>[] includes) where TDtoEntity : class;


        Task<IEnumerable<T>> ListAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          int? offset = null, int? limit = null,
                                          CancellationToken cancellationToken = default,
                                          params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<TDtoEntity>> GetByCriteriaAsync<TDtoEntity>(IExpressionFilter<T> criteria = null,
                                                                     Expression<Func<T, TDtoEntity>> Projection = null,
                                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                     int? offset = null, int? limit = null,
                                                                     CancellationToken cancellationToken = default,
                                                                     params Expression<Func<T, object>>[] includes) where TDtoEntity : class;

        Task<IEnumerable<TDtoEntity>> GetByCriteriaWithManySelectAsync<TDtoEntity>(IExpressionFilter<T> criteria = null,
                                                                     Expression<Func<T, IEnumerable<TDtoEntity>>> Projection = null,
                                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                     int? offset = null, int? limit = null,
                                                                     CancellationToken cancellationToken = default,
                                                                     params Expression<Func<T, object>>[] includes) where TDtoEntity : class;

        Task<IEnumerable<T>> GetByCriteriaAsync(IExpressionFilter<T> criteria = null,
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                int? offset = null, int? limit = null,
                                                CancellationToken cancellationToken = default,
                                                params Expression<Func<T, object>>[] includes);

        IQueryable<T> GetQuery(IExpressionFilter<T> criteria = null,
                               Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                               int? offset = null, int? limit = null,
                               params Expression<Func<T, object>>[] includes);


        Task<int> CountAsync(IExpressionFilter<T> criteria = null, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(IExpressionFilter<T> criteria = null, CancellationToken cancellationToken = default);
    }

}
