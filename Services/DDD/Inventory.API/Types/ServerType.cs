using GraphQL.Types;
using GraphQL.Utilities.Federation;
using Inventory.Domain.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Types
{
    public class ServerType: ObjectGraphType<Server>
    {
        public ServerType()
        {

            Field(s => s.ServerId);
            Field(s => s.HostName).Name("hostname");
            Field(s => s.CPU).Name("cpu");
            Field(s => s.RAM).Name("ram");

            Field<StringGraphType>()
                .Name("os")
                .Resolve(ctx => 
                {
                    return ctx.Source.OperatingSystem.Name;
                });

            Field<OperatingSystemEnum>()
                .Name("osFamilly")
                .Resolve(ctx =>
                {
                    return ctx.Source.OperatingSystem.Familly;
                });

            Field<AnyScalarGraphType>()
                .Name("Variables")
                .Resolve(ctx =>
                {
                    return ctx.Source.GetAnsibleVariables();
                });

        }
    }
}
