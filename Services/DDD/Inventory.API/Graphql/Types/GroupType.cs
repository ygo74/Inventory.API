using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.Domain.Models;
using System.Collections.Generic;
using Inventory.API.Infrastructure;
using Inventory.Domain.Extensions;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using Inventory.Infrastructure.GroupVarsFiles;
using GraphQL.Utilities.Federation;
using System.Text.Json;
using Inventory.API.Application.Dto;

namespace Inventory.API.Graphql.Types
{
    public class GroupType : ObjectGraphType<Group>
    {
        public GroupType(GraphQLService graphQLService, IDataLoaderContextAccessor accessor, InventoryFilesContext inventoryFilesContext)
        {
            Field(g => g.GroupId);
            Field(g => g.Name);
            Field(g => g.AnsibleGroupName).Name("ansible_group_name");


            //Servers
            Field<ListGraphType<ServerType>, IEnumerable<ServerDto>>()
                .Name("Servers")
                .ResolveAsync(ctx =>
                {
                    var itemsloader = accessor.Context.GetOrAddCollectionBatchLoader<int, ServerDto>("GetServersByGroupId", graphQLService.GetServersByGroupAsync);
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

            Field<AnyScalarGraphType>()
                .Name("Variables")
                .Resolve(ctx =>
                {

                    if (!ctx.UserContext.ContainsKey("environment")) return null;


                    var env = ctx.UserContext["environment"];
                    var result = inventoryFilesContext.GetGroupVariables($"/inventories/{env}/group_vars", ctx.Source.AnsibleGroupName);
                    if (result == null) { return null; }
                    return JsonDocument.Parse(result.ToString()).RootElement;

                });


        }
    }

}
