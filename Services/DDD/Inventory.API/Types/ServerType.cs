using GraphQL.Types;
using GraphQL.Utilities.Federation;
using Inventory.API.Dto;
using Inventory.Domain.Extensions;
using Inventory.Domain.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Types
{
    public class ServerType: ObjectGraphType<ServerDto>
    {
        public ServerType()
        {

            //Field(s => s.ServerId);
            //Field(s => s.HostName).Name("hostname");
            //Field(s => s.CPU).Name("cpu");
            //Field(s => s.RAM).Name("ram");

            //Field<StringGraphType>()
            //    .Name("os")
            //    .Resolve(ctx => 
            //    {
            //        return ctx.Source.OperatingSystem.Name;
            //    });

            //Field<OsFamillyEnum>()
            //    .Name("osFamilly")
            //    .Resolve(ctx =>
            //    {
            //        return ctx.Source.OperatingSystem.Familly;
            //    });

            //Field<AnyScalarGraphType>()
            //    .Name("group_names")
            //    .Resolve(ctx =>
            //    {
            //        return ctx.Source.GetInternalServerGroups().Select(g => g.AnsibleGroupName).ToArray();
            //    });

            //Field<AnyScalarGraphType>()
            //    .Name("environments")
            //    .Resolve(ctx =>
            //    {
            //        return ctx.Source.ServerEnvironments.Select(e => e.Environment.Name).ToArray();
            //    });


            //Field<AnyScalarGraphType>()
            //    .Name("Variables")
            //    .Resolve(ctx =>
            //    {
            //        return ctx.Source.GetAnsibleVariables();
            //    });



            Field(s => s.HostName).Name("hostname");

            Field<AnyScalarGraphType>()
                .Name("group_names")
                .Resolve(ctx =>
                {
                    return ctx.Source.Groups.Select(g => g.AnsibleGroupName).ToArray();
                });

            Field<AnyScalarGraphType>()
                .Name("Variables")
                .Resolve(ctx =>
                {
                    return ctx.Source.Variables;
                });


        }
    }
}
