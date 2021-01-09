using Inventory.Domain.Models;
using GraphQL.Types;

namespace Inventory.API.Graphql.Types.Disks
{
    public class WindowsDiskType : ObjectGraphType<WindowsDisk>
    {
        public WindowsDiskType()
        {
            Name = "windowsdisk";

            Field(d => d.Name);
            Field(d => d.Size);
            Field(d => d.Format);

            Interface<DiskInterfaceGraphType>();
            IsTypeOf = d => d is WindowsDisk;
        }
    }
}
