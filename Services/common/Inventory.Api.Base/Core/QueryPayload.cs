using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api.Base.Core
{
    public class QueryPayload<T> : BasePayload<QueryPayload<T>, IApiError>
    {
        public T Result { get; set; }
    }

    public class QueryCursorPaginatedPayload<T> : QueryPayload<IReadOnlyList<T>>
    {
        public int Count { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }

}
