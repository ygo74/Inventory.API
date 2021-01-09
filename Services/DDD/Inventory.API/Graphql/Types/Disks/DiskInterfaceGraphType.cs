using GraphQL.Types;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Graphql.Types.Disks
{
    public class DiskInterfaceGraphType : InterfaceGraphType<BaseDisk>
    {
        public DiskInterfaceGraphType(WindowsDiskType windowsDiskType, LinuxDiskType linuxDiskType)
        {
            Name = "disk";

            Field(d => d.Name);
            Field(d => d.Size);
            Field(d => d.Format);

            ResolveType = @object =>
            {
                return @object switch
                {
                    WindowsDisk => windowsDiskType,
                    LinuxDisk => linuxDiskType,
                    _ => default
                };
            };
        }
    }
}
