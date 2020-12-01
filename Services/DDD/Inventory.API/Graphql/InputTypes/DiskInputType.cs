using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Graphql.InputTypes
{
    public class DiskInputType : InputObjectGraphType
    {
        public DiskInputType()
        {
            Name = nameof(DiskInputType);
            Field<NonNullGraphType<IntGraphType>>().Name("Size");
        }
    }
}
