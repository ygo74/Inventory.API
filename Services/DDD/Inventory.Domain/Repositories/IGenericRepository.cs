using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<T> GetAsync(Int64 Id);
    }
}
