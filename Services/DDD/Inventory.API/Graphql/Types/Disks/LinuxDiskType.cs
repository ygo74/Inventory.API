using Inventory.Domain.Models;
using GraphQL.Types;

namespace Inventory.API.Graphql.Types.Disks
{
    public class LinuxDiskType : ObjectGraphType<LinuxDisk>
    {
        public LinuxDiskType()
        {
            Name = "linuxDisk";

            Field(d => d.Name);
            Field(d => d.Size);
            Field(d => d.Format);

            Interface<DiskInterfaceGraphType>();
            IsTypeOf = d => d is LinuxDisk;
        }
    }
}
