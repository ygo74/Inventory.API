using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Servers.Api.Graphql.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class ServerQueries
    {
        /// <summary>
        /// Return server current date-time
        /// </summary>
        /// <returns>DateTime current date time</returns>
        //public DateTime GetServerDateTime() => DateTime.Now;

        public string GetStatus() => "OK";
    }
}
