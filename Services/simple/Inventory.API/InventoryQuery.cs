using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.Infrastructure;
using Inventory.Domain.Models;
using Inventory.API.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.API.Repository;

namespace Inventory.API
{
    public class InventoryQuery : ObjectGraphType
    {
        private InventoryDbContext _dbContext;

        public InventoryQuery(IDataLoaderContextAccessor accessor, InventoryDbContext dbContext, IInventoryRepository inventoryRepository)
        {

            _dbContext = dbContext;

            Field<ListGraphType<ServerType>, List<Server>>()
                .Name("Servers")
                .ResolveAsync(ctx =>
               {
                   return dbContext.Servers.ToListAsync();
               });


            Field<ListGraphType<GroupType>, List<Group>>()
                .Name("Groups")
                .Argument<StringGraphType>("GroupName")
                .Resolve(ctx =>
                {
                    var groupName = ctx.GetArgument<String>("GroupName");
                    //var allGroups = dbContext.Groups.Include(g => g.Parent).Include(g => g.Children).ToListAsync();

                    if (String.IsNullOrEmpty(groupName))
                    {
                        return dbContext.Groups.Include(g => g.Parent).Include(g => g.Children).ToList();
                    }
                    else
                    {
                        //return allGroups.Where(grp => grp.Name == osName.ToLower()).ToList();
                        //return dbContext.Groups.Where(grp => grp.Name == osName.ToLower())
                        //                       .Include(g => g.Parent)
                        //                       .Include(g => g.Children)
                        //                       .ToListAsync();
                        //var groupQuery = dbContext.Groups.Where(grp => grp.Name == osName.ToLower());
                        //var result = GetItemtree(groupQuery);
                        //return dbContext.Groups.Include(g => g.Parent).Include(g => g.Children).ToListAsync();
                        var parents = inventoryRepository.GetParentGroups(groupName);
                        var childrens = inventoryRepository.GetChildrenGroups(groupName);

                        return parents.Concat(childrens).Distinct().Where(g => g.Name == groupName).ToList();

                    }
                });


        }

    }
}
