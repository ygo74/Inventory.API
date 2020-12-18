using GraphQL.Types;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Graphql.Types
{
    public class TrustLevelType : ObjectGraphType<TrustLevel>
    {
        public TrustLevelType()
        {
            Field(t => t.Name);
            Field(t => t.Code);
        }
    }
}
