using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.Domain.Models;
using Inventory.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Types
{
    public class GroupInputType : InputObjectGraphType<Group>
    {
        public GroupInputType()
        {
            Name = "GroupInput";
            Field<NonNullGraphType<StringGraphType>>("Name");

            Field<GroupInputType>("Parent");
        }
    }

}
