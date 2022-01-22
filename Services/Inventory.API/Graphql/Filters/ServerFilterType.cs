using GraphQL.Types;
using Inventory.Domain.Filters;

namespace Inventory.API.Graphql.Filters
{
    public class ServerFilterType : InputObjectGraphType<ServerFilter>
    {
        public ServerFilterType()
        {
            Field(t => t.Skip);
            Field(t => t.Take);
        }
    }
}
