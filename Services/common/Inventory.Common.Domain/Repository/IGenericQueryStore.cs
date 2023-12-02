using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Domain.Repository
{
    public interface IGenericQueryStore<T> where T : Entity
    {
        Task<TDtoEntity> GetByIdAsync<TDtoEntity>(int id) where TDtoEntity : class;
        Task<T> GetByIdAsync(int id);

        Task<IEnumerable<TDtoEntity>> ListAllAsync<TDtoEntity>(params Expression<Func<T, object>>[] includes) where TDtoEntity : class;
        Task<IEnumerable<T>> ListAllAsync(params Expression<Func<T, object>>[] includes);


        Task<IEnumerable<TDtoEntity>> GetByCriteriaAsync<TDtoEntity>(IExpressionFilter<T> criteria = null) where TDtoEntity: class;
        Task<IEnumerable<T>> GetByCriteriaAsync(IExpressionFilter<T> criteria = null);
    }

}
