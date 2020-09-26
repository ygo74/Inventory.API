using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API
{
    public class InventorySchema : Schema
    {
        public InventorySchema(IServiceProvider resolver) : base(resolver)
        {
            Query = resolver.GetRequiredService<InventoryQuery>();
            Mutation = resolver.GetRequiredService<InventoryMutation>();
        }

    }
}
