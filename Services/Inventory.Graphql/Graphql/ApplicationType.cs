using HotChocolate.Types;
using Inventory.Domain.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Graphql.Graphql
{
    public class ApplicationType : ObjectType<Inventory.Domain.Models.Configuration.Application>
    {
        protected override void Configure(IObjectTypeDescriptor<Application> descriptor)
        {
            descriptor.BindFields(BindingBehavior.Implicit);
        }
    }
}
