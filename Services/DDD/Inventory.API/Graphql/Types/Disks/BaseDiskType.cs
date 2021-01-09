using GraphQL.Types;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Graphql.Types.Disks
{
    public class BaseDiskType : ObjectGraphType<BaseDisk>
    {
        public BaseDiskType()
        {
            Name = "baseDisk";

            Field(d => d.Name);
            Field(d => d.Size);
            Field(d => d.Format);

            Interface<DiskInterfaceGraphType>();
            IsTypeOf = d => d is BaseDisk;

        }
    }
}
