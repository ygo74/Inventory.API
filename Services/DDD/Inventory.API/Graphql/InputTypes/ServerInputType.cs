using GraphQL.Types;
using Inventory.API.Graphql.InputTypes;
using Inventory.API.Graphql.Types;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inventory.API.Graphql.InputTypes
{
    public class ServerInputType : InputObjectGraphType
    {
        public ServerInputType()
        {
            Name = "ServerInput";
            Field< NonNullGraphType<StringGraphType>>().Name("hostname");
            Field<OsFamillyEnum>("os_familly");
            Field<NonNullGraphType<StringGraphType>>().Name("os");
            Field<NonNullGraphType<StringGraphType>>().Name("environment");
            Field<NonNullGraphType<StringGraphType>>().Name("subnetIP");

            Field<ListGraphType<DiskInputType>>().Name("Disks");

            Field<ListGraphType<StringGraphType>>().Name("Groups");

        }
    }
}
