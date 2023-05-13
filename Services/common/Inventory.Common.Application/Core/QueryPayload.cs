using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Core
{
    public class CursorPaginationdPayload<T> : Payload<IReadOnlyList<T>>
    {
        public int TotalCount { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public string StartCursor { get; set; }
        public string EndCursor { get; set; }
    }

    public class OffsetPaginationPayload<T> : Payload<IReadOnlyList<T>>
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int PageCount { get; set; }
    }

}
