using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.Domain.Models;
using Inventory.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Utilities.Federation;
using System.Text.Json;
using Inventory.Infrastructure.GroupVarsFiles;

namespace Inventory.API.Types
{
    public class GroupType : ObjectGraphType<Group>
    {
        public GroupType(IInventoryRepository inventoryRepository, IInventoryFilesContext inventoryFilesContext, IDataLoaderContextAccessor accessor)
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

            //Variables
            //Field<ListGraphType<VariableType>, IEnumerable<Variable>>()
            //    .Name("Variables")
            //    .Resolve(ctx =>
            //    {
            //        return ctx.Source.Variables;
            //    });

            //Field<AnyScalarGraphType>()
            //    .Name("Variables")
            //    .Resolve(ctx =>
            //    {
            //        //var sv = new StringVariable() { Name = "a", Value = "a" };
            //        //var nv = new NumericVariable() { Name = "b", Value = 1 };
            //        //var lv = new List<Variable>() { sv, nv };
            //        //return lv;
            //        //return ctx.Source.Variables;

            //        JsonDocument doc = JsonDocument.Parse(@"
            //            {
            //                ""test"": 1,
            //                ""test2"": [""x"",""y""]
            //            }
            //        "
            //        );

            //        return doc.RootElement;

            //    });

            Field<AnyScalarGraphType>()
                .Name("Variables")
                .Resolve(ctx =>
                {
                    //var sv = new StringVariable() { Name = "a", Value = "a" };
                    //var nv = new NumericVariable() { Name = "b", Value = 1 };
                    //var lv = new List<Variable>() { sv, nv };
                    //return lv;
                    //return ctx.Source.Variables;

                    var variables = inventoryFilesContext.GetVariables(@"/inventories/poc/group_vars");

                    return JsonDocument.Parse(variables["all"].ToString()).RootElement;

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
