using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.API.Graphql.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Domain.Specifications;

namespace Inventory.API
{
    public class GroupQuery : ObjectGraphType
    {
        public GroupQuery(IDataLoaderContextAccessor accessor, IAsyncRepository<Group> groupRepository)
        {

            Name = "Group";

            Field<ListGraphType<GroupType>, IReadOnlyList<Group>>()
                .Name("Groups")
                .ResolveAsync(ctx =>
               {
                   return groupRepository.ListAsync(new GroupSpecification());
               });
        }
    }
}
