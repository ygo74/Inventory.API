using Inventory.Domain.Models;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class ServerRepository : GenericRepository<Server>
    {
        public ServerRepository(DbContext dbContext) : base(dbContext) { }
    }
}
