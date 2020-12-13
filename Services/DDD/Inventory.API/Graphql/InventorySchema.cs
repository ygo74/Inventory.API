using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.API.Graphql.Extensions;
using Inventory.API.Graphql.Queries;
using Inventory.API.Graphql.Mutations;

namespace Inventory.API.Graphql
{
    public class InventorySchema : Schema
    {
        public InventorySchema(IServiceProvider resolver) : base(resolver)
        {
            this.AddQuery<InventoryQuery>(resolver);
            this.AddQuery<ServerQuery>(resolver);
            this.AddQuery<GroupQuery>(resolver);

            this.AddMutation<ServerMutation>(resolver);
        }

    }
}
