using GraphQL.Types;
using Inventory.Domain.Models;
using Inventory.Domain.Models.Configuration;
using Inventory.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Graphql.Types
{
    public class LocationType : ObjectGraphType<Location>
    {
        public LocationType()
        {
            Field(l => l.Name);
            Field(l => l.CountryCode);
            Field(l => l.CityCode);
        }
    }
}
