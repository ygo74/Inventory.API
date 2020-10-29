using GraphQL.Types;

namespace Inventory.API.Types
{
    public class ServerInputType : InputObjectGraphType
    {
        public ServerInputType()
        {
            Name = "ServerInput";
            Field< NonNullGraphType<StringGraphType>>("Name");
            Field<NonNullGraphType<StringGraphType>>("NetworkLocation");
            Field<OperatingSystemEnum>("OperatingSystem");
        }
    }
}
