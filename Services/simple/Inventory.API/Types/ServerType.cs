using GraphQL.Types;
using Inventory.API.Models;
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
            Field(s => s.Name);
            //Field(s => s.OperatingSystem);
            Field<OperatingSystemEnum>(nameof(Server.OperatingSystem));

        }
    }
}
