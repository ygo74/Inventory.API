using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Graphql.InputTypes
{
    public class GroupInputType : InputObjectGraphType
    {
        public GroupInputType()
        {
            Field<NonNullGraphType<StringGraphType>>().Name("name");
            Field<NonNullGraphType<StringGraphType>>().Name("ansibleGroupName");
            Field<StringGraphType>().Name("parentName");
        }
    }
}
