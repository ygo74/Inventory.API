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
using GraphQL;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.API.Graphql.Queries
{
    public class GroupQuery : ObjectGraphType
    {
        public GroupQuery(IDataLoaderContextAccessor accessor, IGroupRepository groupRepository)
        {

            Field<ListGraphType<GroupType>, IReadOnlyList<Group>>()
                .Name("Groups")
                .ResolveAsync(ctx =>
                {
                    return groupRepository.ListAsync(new GroupSpecification());
                });

            Field<ListGraphType<GroupType>, IReadOnlyList<Group>>()
                .Name("GroupByName")
                .Argument<NonNullGraphType<StringGraphType>>("GroupName")
                .Resolve(ctx =>
                {
                    var groupName = ctx.GetArgument<String>("GroupName");
                    return groupRepository.GetAllLinkedGroups(groupName);

                });

        }
    }
}
