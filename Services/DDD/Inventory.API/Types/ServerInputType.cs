using GraphQL.Types;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inventory.API.Types
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

        }
    }
}
