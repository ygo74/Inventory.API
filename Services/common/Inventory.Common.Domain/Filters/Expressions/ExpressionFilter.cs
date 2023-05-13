using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Domain.Filters
{
    /// <summary>
    /// from https://github.com/bmgandre/dotnet-specification-pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ExpressionFilter<T> : IExpressionFilter<T>
    {
        public abstract Expression<Func<T, bool>> Predicate {get;}

        public IExpressionFilter<T> And(IExpressionFilter<T> filterCriteria)
        {
            return new AndFilterCriteria<T>(this, filterCriteria);
        }

        public IExpressionFilter<T> And(Expression<Func<T, bool>> right)
        {
            Guard.Against.Null(right, nameof(right));
            return new AndFilterCriteria<T>(this, new ExpressionFilterBuilder<T>(right));
        }

        public IExpressionFilter<T> Init(Expression<Func<T, bool>> expression)
        {
            return new ExpressionFilterBuilder<T>(expression);
        }

        public IExpressionFilter<T> InitEmpty()
        {
            return new NullFilterCriteria<T>();
        }


        public IExpressionFilter<T> Not()
        {
            return new NotFilterCriteria<T>(this);
        }

        public IExpressionFilter<T> Or(IExpressionFilter<T> filterCriteria)
        {
            return new OrFilterCriteria<T>(this, filterCriteria);
        }

        public IExpressionFilter<T> Or(Expression<Func<T, bool>> right)
        {
            Guard.Against.Null(right, nameof(right));
            return new OrFilterCriteria<T>(this, new ExpressionFilterBuilder<T>(right));
        }

    }

    public class NullFilterCriteria<T> : ExpressionFilter<T>
    {
        public override Expression<Func<T, bool>> Predicate { get; }

        public NullFilterCriteria() { }
    }

    public class ExpressionFilterBuilder<T> : ExpressionFilter<T>
    {
        private readonly Expression<Func<T, bool>> _predicate;

        public override Expression<Func<T, bool>> Predicate { get => _predicate; }

        public ExpressionFilterBuilder(Expression<Func<T, bool>> predicate)
        {
            _predicate = predicate;
        }
    }

    internal class SwapVisitor : ExpressionVisitor
    {
        private readonly Expression from, to;

        public SwapVisitor(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }

        public override Expression Visit(Expression node)
        {
            return node == from ? to : base.Visit(node);
        }
    }

    public class AndFilterCriteria<T> : ExpressionFilter<T>
    {
        private readonly IExpressionFilter<T> _left;
        private readonly IExpressionFilter<T> _right;

        public override Expression<Func<T, bool>> Predicate
        {
            get
            {
                return _left.Predicate != null ? And(_left.Predicate, _right.Predicate) : _right.Predicate;
            }
        }

        public AndFilterCriteria(IExpressionFilter<T> left, IExpressionFilter<T> right)
        {
            _left = left;
            _right = right ?? throw new ArgumentNullException(nameof(right));
        }

        private static Expression<Func<T, bool>> And(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            var visitor = new SwapVisitor(left.Parameters[0], right.Parameters[0]);
            var binaryExpression = Expression.AndAlso(visitor.Visit(left.Body), right.Body);
            var lambda = Expression.Lambda<Func<T, bool>>(binaryExpression, right.Parameters);
            return lambda;
        }
    }

    public class OrFilterCriteria<T> : ExpressionFilter<T>
    {
        private readonly IExpressionFilter<T> _left;
        private readonly IExpressionFilter<T> _right;

        public override Expression<Func<T, bool>> Predicate
        {
            get
            {
                return _left.Predicate != null ? Or(_left.Predicate, _right.Predicate) : _right.Predicate;
            }
        }

        public OrFilterCriteria(IExpressionFilter<T> left, IExpressionFilter<T> right)
        {
            _left = left;
            _right = right ?? throw new ArgumentNullException(nameof(right));
        }

        private static Expression<Func<T, bool>> Or(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            var visitor = new SwapVisitor(left.Parameters[0], right.Parameters[0]);
            var binaryExpression = Expression.OrElse(visitor.Visit(left.Body), right.Body);
            var lambda = Expression.Lambda<Func<T, bool>>(binaryExpression, right.Parameters);
            return lambda;
        }
    }

    public class NotFilterCriteria<T> : ExpressionFilter<T>
    {
        private readonly IExpressionFilter<T> _left;

        public override Expression<Func<T, bool>> Predicate
        {
            get
            {
                return Not(_left.Predicate);
            }
        }

        public NotFilterCriteria(IExpressionFilter<T> left)
        {
            _left = left ?? throw new ArgumentNullException(nameof(left));
        }

        private static Expression<Func<T, bool>> Not(Expression<Func<T, bool>> left)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            var notExpression = Expression.Not(left.Body);
            var lambda = Expression.Lambda<Func<T, bool>>(notExpression, left.Parameters.Single());
            return lambda;
        }
    }

}
