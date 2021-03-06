﻿using GraphQL.Types;
using GraphQL.Utilities.Federation;
using Inventory.API.Application.Dto;
using Inventory.API.Graphql.Types.Disks;
using System.Linq;

namespace Inventory.API.Graphql.Types
{
    public class ServerType: ObjectGraphType<ServerDto>
    {
        public ServerType()
        {

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
            Field(s => s.CPU).Name("cpu");
            Field(s => s.RAM).Name("ram");

            Field<StringGraphType>()
                .Name("os")
                .Resolve(ctx =>
                {
                    return ctx.Source.OperatingSystem.Name;
                });

            Field<OsFamillyEnum>()
                .Name("osFamilly")
                .Resolve(ctx =>
                {
                    return ctx.Source.OperatingSystem.Familly;
                });

            Field<LocationType>()
                .Name("location")
                .Resolve(ctx =>
                {
                    return ctx.Source.Location;
                });

            Field<AnyScalarGraphType>()
                .Name("group_names")
                .Resolve(ctx =>
                {
                    return ctx.Source.Groups.Select(g => g.AnsibleGroupName).ToArray();
                });

            Field<AnyScalarGraphType>()
                .Name("environments")
                .Resolve(ctx =>
                {
                    return ctx.Source.Environments.Select(e => e.Name).ToArray();
                });


            Field<ListGraphType<DiskInterfaceGraphType>>()
                .Name("disks")
                .Resolve(ctx =>
                {
                    return ctx.Source.Disks;
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
