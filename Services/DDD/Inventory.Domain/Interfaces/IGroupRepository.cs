using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Repositories.Interfaces
{
    public interface IGroupRepository : IAsyncRepository<Group>
    {
    }
}
