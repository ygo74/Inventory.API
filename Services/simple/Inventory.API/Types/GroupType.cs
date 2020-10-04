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
    public class GroupType : ObjectGraphType<Group>
    {
        public GroupType(IInventoryRepository inventoryRepository, IDataLoaderContextAccessor accessor)
        {
            Field(g => g.GroupId);
            Field(g => g.Name);

            //Servers
            Field<ListGraphType<ServerType>, IEnumerable<Server>>()
                .Name("Servers")
                .ResolveAsync(ctx =>
                {
                    var itemsloader = accessor.Context.GetOrAddCollectionBatchLoader<int, Server>("GetServersByGroupId", inventoryRepository.GetServersByGroupAsync);
                    return itemsloader.LoadAsync(ctx.Source.GroupId);

                });

            //Parent
            Field<ListGraphType<GroupType>, IEnumerable<Group>>()
                .Name("Parents")
                .Resolve(ctx =>
                {
                    if (null == ctx.Source.Parent) { return null; }
                    return ctx.Source.Parent.TraverseParents();
                });

            //Children
            Field<ListGraphType<GroupType>, IEnumerable<Group>>()
                .Name("Children")
                .Resolve(ctx =>
                {
                    return ctx.Source.Children;
                });

        }
    }

    public static class FlattenExtension
    {
        public static IEnumerable<Group> TraverseParents(this Group entity)
        {
            var stack = new Stack<Group>();
            stack.Push(entity);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;
                if (null != current.Parent)
                {
                    stack.Push(current.Parent);
                }

            }

        }
    }

}
