using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Inventory.Common.Domain.Repository
{
    public interface IAsyncRepository<T> : IRepositoryBase<T> where T : class
    {
        IUnitOfWork UnitOfWork { get; }

        Task<T> FirstAsync(ISpecification<T> spec);
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
    }
}
