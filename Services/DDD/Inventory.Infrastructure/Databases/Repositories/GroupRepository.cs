using Inventory.Domain.Models;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Databases.Repositories
{
    public class GroupRepository : GenericRepository<Group>
    {
        public GroupRepository(DbContext dbContext) : base(dbContext) { }

    }
}
