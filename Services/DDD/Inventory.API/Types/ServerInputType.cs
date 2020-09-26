using GraphQL.Types;
using Inventory.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
