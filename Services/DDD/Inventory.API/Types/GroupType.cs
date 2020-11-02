using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.Domain.Models;
using System.Collections.Generic;
using Inventory.API.Infrastructure;
using Inventory.Domain.Extensions;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace Inventory.API.Types
{
    public class GroupType : ObjectGraphType<Group>
    {
        public GroupType(GraphQLService graphQLService, IDataLoaderContextAccessor accessor)
        {
            Field(g => g.GroupId);
            Field(g => g.Name);
            Field(g => g.AnsibleGroupName).Name("ansible_group_name");


            //Servers
            Field<ListGraphType<ServerType>, IEnumerable<Server>>()
                .Name("Servers")
                .ResolveAsync(ctx =>
                {
                    var itemsloader = accessor.Context.GetOrAddCollectionBatchLoader<int, Server>("GetServersByGroupId", graphQLService.GetServersByGroupAsync);
                    return itemsloader.LoadAsync(ctx.Source.GroupId);

                });

            //Parent
            Field<GroupType, Group>()
                .Name("Parent")
                .Resolve(ctx =>
                {
                    if (null == ctx.Source.Parent) { return null; }
                    return ctx.Source.Parent;
                });

            //Parents
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
                    if (null == ctx.Source.Children) { return null; }
                    return ctx.Source.Children;
                });

            //Childrens
            Field<ListGraphType<GroupType>, IEnumerable<Group>>()
                .Name("Childrens")
                .Resolve(ctx =>
                {
                    List<Group> allChildrendGroups = new List<Group>();
                    foreach (Group childGroup in ctx.Source.Children)
                    {
                        allChildrendGroups.Add(childGroup);
                        allChildrendGroups.AddRange(childGroup.FlattenChildrends());
                    }

                    return allChildrendGroups.Distinct().ToList();
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

            //Field<AnyScalarGraphType>()
            //    .Name("Variables")
            //    .Resolve(ctx =>
            //    {
            //        //var sv = new StringVariable() { Name = "a", Value = "a" };
            //        //var nv = new NumericVariable() { Name = "b", Value = 1 };
            //        //var lv = new List<Variable>() { sv, nv };
            //        //return lv;
            //        //return ctx.Source.Variables;

            //        var variables = inventoryFilesContext.GetVariables(@"/inventories/poc/group_vars");

            //        return JsonDocument.Parse(variables["all"].ToString()).RootElement;

            //    });


        }
    }

}
