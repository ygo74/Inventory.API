using HotChocolate;
using Inventory.Infrastructure.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Graphql.Graphql
{
    public class Query
    {
        public IQueryable<Inventory.Domain.Models.Configuration.Application> GetApplications([Service] InventoryDbContext context) =>
            context.Applications;
    }
}
