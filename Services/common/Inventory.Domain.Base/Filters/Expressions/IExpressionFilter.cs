using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Base.Filters
{

    public interface IExpressionFilter<T>
    {
        Expression<Func<T, bool>> Predicate { get; }

        //bool IsSatisfiedBy(T entity);

        //IQueryable<T> Prepare(IQueryable<T> query);

        //T SatisfyingItemFrom(IQueryable<T> query);

        //IQueryable<T> SatisfyingItemsFrom(IQueryable<T> query);

        IExpressionFilter<T> Init(Expression<Func<T, bool>> expression);

        IExpressionFilter<T> InitEmpty();

        IExpressionFilter<T> And(IExpressionFilter<T> filterCriteria);

        IExpressionFilter<T> And(Expression<Func<T, bool>> right);

        IExpressionFilter<T> Or(IExpressionFilter<T> filterCriteria);

        IExpressionFilter<T> Or(Expression<Func<T, bool>> right);

        IExpressionFilter<T> Not();
    }
}
