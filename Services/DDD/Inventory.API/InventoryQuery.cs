﻿using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.API.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Domain.Specifications;
using GraphQL;

namespace Inventory.API
{
    public class InventoryQuery : ObjectGraphType
    {
        public InventoryQuery(IAsyncRepository<Server> serverRepository, IGroupRepository groupRepository)
        {
            Name = "Query";
            //Field<ServerQuery>("server", resolve: context => new { });
            //Field<GroupQuery> ("group", resolve: context => new { });

            Field<ListGraphType<ServerType>, IReadOnlyList<Server>>()
                .Name("Servers")
                .ResolveAsync(ctx =>
                {
                    return serverRepository.ListAsync(new ServerSpecification());
                });

            Field<ListGraphType<GroupType>, IReadOnlyList<Group>>()
                .Name("Groups")
                .ResolveAsync(ctx =>
                {
                    return groupRepository.ListAsync(new GroupSpecification());
                });

            Field<GroupType, Group>()
                .Name("GroupByName")
                .Argument<NonNullGraphType<StringGraphType>>("GroupName")
                .Resolve(ctx =>
                {
                    var groupName = ctx.GetArgument<String>("GroupName");
                    return groupRepository.GetAllLinkedGroups(groupName).SingleOrDefault(g => g.Name == groupName);

                });

        }
    }
}
